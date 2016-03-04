using UnityEngine;
using System.Collections;
using Notifications;
using Model;
using Model.Data;
using Model.Decorators;
using System;
using System.Collections.Generic;
using UnityEngine.Analytics;
using Utils;
using UnityEngine.SceneManagement;

namespace Controllers
{
	public class GameController : MonoBehaviour
	{				
		//current maze data
		MazeModel _mazeData;
		
		//current game state
		GameStateModel _gameState;

		//starting point of the maze
		IntPoint _mazeStartPos;
		DelayedValue _scoreDelayed;
		DelayedValue _movesDelayed;
		DelayedValue _timeBonusDelayed;
		
		// Use this for initialization
		void Start ()
		{
			_mazeStartPos = new IntPoint (0, 0);

			_gameState.state = GameStateModel.STATE_INVALID;
			_gameState.movesLeft = 0;
			_gameState.score = 0;
			_gameState.levelNumber = PlayerPrefs.GetInt ("maxlevel", 0) / 2;
			Next ();

			MazePaceNotifications.PLAYER_READY_TO_PROCEED.Add (OnReadyToProceed);
			MazePaceNotifications.PLAYER_STUCK.Add (OnStuck);
		}
		
		// Update is called once per frame
		void Update ()
		{
			switch (_gameState.state) {
			//if not activated - drain timebonus;
			case (GameStateModel.STATE_INITED): 
				_gameState.timeBonus = _timeBonusDelayed.GetCurrentValue (); 
				MazePaceNotifications.GAME_STATE_UPDATED.Dispatch(_gameState);
				break;
			//if stuck - drain score
			case(GameStateModel.STATE_STUCK):
				_gameState.score = _scoreDelayed.GetCurrentValueAsInt ();
				if (_gameState.score <= 0) {
					_gameState.score = 0;
					AnalyticsWrapper.ReportGameLost (_gameState);
					SceneManager.LoadScene ("MenuScene");
				} 
				MazePaceNotifications.GAME_STATE_UPDATED.Dispatch(_gameState);
				break;
			//if ended - transfer moves to score in 0.5 seconds
			case(GameStateModel.STATE_ENDED):
				_gameState.score = _scoreDelayed.GetCurrentValueAsInt ();
				if (_gameState.maxScore < _gameState.score)
					_gameState.maxScore = _gameState.score;
				_gameState.movesLeft = _movesDelayed.GetCurrentValueAsUInt ();
				MazePaceNotifications.GAME_STATE_UPDATED.Dispatch(_gameState);
				break;
			}
		}
		
		void OnApplicationPause (bool paused)
		{
			AnalyticsWrapper.ReportGamePaused (_gameState);
		
			if (_gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
				PlayerPrefs.SetInt ("highscore", _gameState.maxScore);
		}
		
		void OnReadyToProceed (IntPoint pos, int directionIdx)
		{		
			NodeVO node = _mazeData.GetNode (pos.x, pos.y);
			
			_gameState.score += (int)((float)node.score * _gameState.timeBonus);
			
			if (_gameState.maxScore < _gameState.score) {
				_gameState.maxScore = _gameState.score;
			}
			
			_gameState.movesLeft--;
					
			if (_gameState.movesLeft == 0) {
				
				if (_gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
					PlayerPrefs.SetInt ("highscore", _gameState.maxScore);
				
				AnalyticsWrapper.ReportGameLost (_gameState);
				
				SceneManager.LoadScene ("MenuScene");
				return;
			} 
				
			if (_gameState.state == GameStateModel.STATE_INITED || _gameState.state == GameStateModel.STATE_STUCK){
				_gameState.state = GameStateModel.STATE_ACTIVATED;	
			}
			
			MazePaceNotifications.GAME_STATE_UPDATED.Dispatch(_gameState);
			
			if (node.HasFlag (NodeVO.SPECIALS_EXIT)) {
				OnExit (pos);
			} else {
				float moveTime = LevelModel.Instance().moveTime;
				if (node.HasFlag (NodeVO.SPECIALS_SPEEDUP_UP)) {
					if (directionIdx == NodeVO.DIRECTION_UP_IDX)
						moveTime /= 2;

					if (directionIdx == NodeVO.DIRECTION_DOWN_IDX)
						moveTime *= 2;
				}

				if (node.HasFlag (NodeVO.SPECIALS_SPEEDUP_RIGHT)) {
					if (directionIdx == NodeVO.DIRECTION_RIGHT_IDX)
						moveTime /= 2;

					if (directionIdx == NodeVO.DIRECTION_LEFT_IDX)
						moveTime *= 2;
				}

				if (node.HasFlag (NodeVO.SPECIALS_SPEEDUP_DOWN)) {
					if (directionIdx == NodeVO.DIRECTION_DOWN_IDX)
						moveTime /= 2;

					if (directionIdx == NodeVO.DIRECTION_UP_IDX)
						moveTime *= 2;
				}

				if (node.HasFlag (NodeVO.SPECIALS_SPEEDUP_LEFT)) {
					if (directionIdx == NodeVO.DIRECTION_LEFT_IDX)
						moveTime /= 2;

					if (directionIdx == NodeVO.DIRECTION_RIGHT_IDX)
						moveTime *= 2;
				}

				if (!node.HasWall (directionIdx)) {
					MazePaceNotifications.PROCEED.Dispatch (node, moveTime);
				} else {
					MazePaceNotifications.PLAYER_STUCK.Dispatch ();
				}
			}
		}
		
		public void OnExitClick ()
		{
			AnalyticsWrapper.ReportGameExit (_gameState);
			SceneManager.LoadScene ("MenuScene");
		}

		void OnExit (IntPoint pos)
		{
			_gameState.state = GameStateModel.STATE_ENDED;
			_movesDelayed = new DelayedValue (_gameState.movesLeft, 0, 0.5f);
			var avgScore = (LevelModel.Instance().minScore + LevelModel.Instance().maxScore) / 2;
			_scoreDelayed = new DelayedValue (_gameState.score, _gameState.score + _gameState.movesLeft * _gameState.timeBonus * avgScore, 0.5f);
			_gameState.levelNumber ++;
			MazePaceNotifications.GAME_STATE_UPDATED.Dispatch(_gameState);

			_mazeStartPos = pos;
			MazePaceNotifications.EXIT_REACHED.Dispatch ();
			Invoke ("Next", 0.6f);
		}

		void OnStuck ()
		{	
			_gameState.state = GameStateModel.STATE_STUCK;
			_scoreDelayed = new DelayedValue (_gameState.score, 0, LevelModel.Instance().scoreDrainTime);
		}
		
		void Next ()
		{	
			if (_gameState.levelNumber > PlayerPrefs.GetInt ("maxlevel", 0))
				PlayerPrefs.SetInt ("maxlevel", _gameState.levelNumber);
		
			MazeModel.Instance().Recreate(LevelModel.Instance().width, LevelModel.Instance().height, _mazeStartPos.x, _mazeStartPos.y);
			ExitDecorator.Apply (_mazeData);
			ScoreDecorator.Apply (_mazeData);
			HiderDecorator.Apply (_mazeData);
			SpeedUpDecorator.Apply (_mazeData);
			RotatorDecorator.Apply (_mazeData);

			MazePaceNotifications.MAZE_RECREATED.Dispatch (_mazeData);

			_gameState.state = GameStateModel.STATE_INITED;
			_gameState.timeBonus = LevelModel.Instance().maxTimeBonus;
			_timeBonusDelayed = new DelayedValue (LevelModel.Instance().maxTimeBonus, LevelModel.Instance().minTimeBonus, LevelModel.Instance().bonusTime);
			_gameState.movesLeft = _mazeData.deadEnds [0].GetDistance () * 2;

			MazePaceNotifications.GAME_STATE_UPDATED.Dispatch(_gameState);
		}

		public void OnDestroy(){
			MazePaceNotifications.PLAYER_READY_TO_PROCEED.Remove (OnReadyToProceed);
			MazePaceNotifications.PLAYER_STUCK.Remove (OnStuck);
		}
	}
}