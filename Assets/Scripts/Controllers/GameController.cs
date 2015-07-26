using UnityEngine;
using System.Collections;
using Models;
using Models.Data;
using Models.Decorators;
using System;
using System.Collections.Generic;
using UnityEngine.Analytics;
using Utils;

namespace Controllers
{
	public class GameController : MonoBehaviour
	{
		GameObject _HUD;
					
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
			Prefabs.Init ();
			_HUD = (GameObject)GameObject.Find ("HUD");
		
			_mazeStartPos = new IntPoint (0, 0);
			_gameState.movesLeft = 0;
			_gameState.score = 0;
			_gameState.levelNumber = PlayerPrefs.GetInt ("maxlevel", 0) / 2;
			Next ();
		}
		
		// Update is called once per frame
		void Update ()
		{
			switch (_gameState.state) {
			//if not activated - drain timebonus;
			case (GameStateModel.STATE_INITED): 
				_gameState.timeBonus = _timeBonusDelayed.GetCurrentValue (); 
				_HUD.BroadcastMessage ("OnGameStateUpdated", _gameState);
				break;
			//if stuck - drain score
			case(GameStateModel.STATE_STUCK):
				_gameState.score = _scoreDelayed.GetCurrentValueAsInt ();
				if (_gameState.score <= 0) {
					_gameState.score = 0;
					if (_gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
						PlayerPrefs.SetInt ("highscore", _gameState.maxScore);
							
					AnalyticsWrapper.ReportGameLost (_gameState);
					Application.LoadLevel ("MenuScene");
				} 
				_HUD.BroadcastMessage ("OnGameStateUpdated", _gameState);
				break;
			//if ended - transfer moves to score in 0.5 seconds
			case(GameStateModel.STATE_ENDED):
				_gameState.score = _scoreDelayed.GetCurrentValueAsInt ();
				if (_gameState.maxScore < _gameState.score)
					_gameState.maxScore = _gameState.score;
				_gameState.movesLeft = _movesDelayed.GetCurrentValueAsUInt ();
				_HUD.BroadcastMessage ("OnGameStateUpdated", _gameState);
				break;
			}
		}
		
		void OnApplicationPause (bool paused)
		{
			AnalyticsWrapper.ReportGamePaused (_gameState);
		
			if (_gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
				PlayerPrefs.SetInt ("highscore", _gameState.maxScore);
		}
		
		void OnStepComplete (IntPoint pos)
		{
			if (_gameState.state == GameStateModel.STATE_INITED || _gameState.state == GameStateModel.STATE_STUCK)
				_gameState.state = GameStateModel.STATE_ACTIVATED;	
			
			NodeModel node = _mazeData.GetNode (pos.x, pos.y);
			
			_gameState.score += (int)((float)node.score * _gameState.timeBonus);
			
			if (_gameState.maxScore < _gameState.score) {
				_gameState.maxScore = _gameState.score;
			}
			
			_gameState.movesLeft--;
					
			if (_gameState.movesLeft == 0) {
				
				if (_gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
					PlayerPrefs.SetInt ("highscore", _gameState.maxScore);
				
				AnalyticsWrapper.ReportGameLost (_gameState);
				
				Application.LoadLevel ("MenuScene");
				return;
			} 

			BroadcastMessage ("onNodeReached", node);
			_HUD.BroadcastMessage ("OnGameStateUpdated", _gameState);
			
			if (node.HasFlag (NodeModel.SPECIALS_EXIT)) {
				onExit (pos);
				return;
			}
		}

		void onExit (IntPoint pos)
		{
			_gameState.state = GameStateModel.STATE_ENDED;
			_movesDelayed = new DelayedValue (_gameState.movesLeft, 0, 0.5f);
			var avgScore = (_mazeData.config.minScore + _mazeData.config.maxScore) / 2;
			_scoreDelayed = new DelayedValue (_gameState.score, _gameState.score + _gameState.movesLeft * _gameState.timeBonus * avgScore, 0.5f);
			_gameState.levelNumber ++;
			_HUD.BroadcastMessage ("OnGameStateUpdated", _gameState);

			_mazeStartPos = pos;
			Invoke ("Next", 0.6f);
		}

		void onStuck ()
		{	
			_gameState.state = GameStateModel.STATE_STUCK;
			_scoreDelayed = new DelayedValue (_gameState.score, 0, _mazeData.config.scoreDrainTime);
		}
		
		void Next ()
		{	
			if (_gameState.levelNumber > PlayerPrefs.GetInt ("maxlevel", 0))
				PlayerPrefs.SetInt ("maxlevel", _gameState.levelNumber);
		
			_gameState.state = GameStateModel.STATE_INITED;
		
			_mazeData = new MazeModel (new MazeConfig (_gameState.levelNumber), _mazeStartPos.x, _mazeStartPos.y);
			
			ExitDecorator.Apply (_mazeData);
			ScoreDecorator.Apply (_mazeData);
			HiderDecorator.Apply (_mazeData);
			SpeedUpDecorator.Apply (_mazeData);
			RotatorDecorator.Apply (_mazeData);
			
			BroadcastMessage ("UpdateMazeData", _mazeData);
			
			_gameState.timeBonus = _mazeData.config.maxTimeBonus;
			_timeBonusDelayed = new DelayedValue (_mazeData.config.maxTimeBonus, _mazeData.config.minTimeBonus, _mazeData.config.bonusTime);
			_gameState.movesLeft = _mazeData.deadEnds [0].GetDistance () * 2;
			
			_HUD.BroadcastMessage ("OnGameStateUpdated", _gameState);
		}
	}
}