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

namespace View
{
	public class GameController : MonoBehaviour
	{	
		// Use this for initialization
		void Start ()
		{
			LevelModel.Instance().SetNumber(PlayerPrefs.GetInt ("maxlevel", 0) / 2);
			StartLevel ();

			MazePaceNotifications.PLAYER_READY_TO_PROCEED.Add (OnReadyToProceed);
			MazePaceNotifications.PLAYER_STUCK.Add (OnStuck);
		}

		void StartLevel ()
		{	
			if (GameStateModel.Instance().levelNumber > PlayerPrefs.GetInt ("maxlevel", 0))
				PlayerPrefs.SetInt ("maxlevel", GameStateModel.Instance().levelNumber);

			MazeModel.Instance ().Recreate (LevelModel.Instance ().width, LevelModel.Instance ().height, PlayerModel.Instance().cellPosition.x, PlayerModel.Instance().cellPosition.y);
			ExitDecorator.Apply (MazeModel.Instance());
			ScoreDecorator.Apply (MazeModel.Instance());
			HiderDecorator.Apply (MazeModel.Instance());
			SpeedUpDecorator.Apply (MazeModel.Instance());
			RotatorDecorator.Apply (MazeModel.Instance());

			MazePaceNotifications.MAZE_RECREATED.Dispatch (MazeModel.Instance());

			GameStateModel.Instance().state = GameStateModel.STATE_INITED;
			GameStateModel.Instance().timeBonus.SetValue (LevelModel.Instance ().maxTimeBonus, LevelModel.Instance ().minTimeBonus, LevelModel.Instance ().bonusTime);
			GameStateModel.Instance().movesLeft.SetValue (MazeModel.Instance().deadEnds [0].GetDistance () * 2);

			MazePaceNotifications.GAME_STATE_UPDATED.Dispatch (GameStateModel.Instance());
		}


		void OnApplicationPause (bool paused)
		{
			AnalyticsWrapper.ReportGamePaused (GameStateModel.Instance());
		
			if (GameStateModel.Instance().maxScore > PlayerPrefs.GetInt ("highscore", 0))
				PlayerPrefs.SetInt ("highscore", GameStateModel.Instance().maxScore);
		}

		void OnReadyToProceed (IntPointVO pos, int directionIdx)
		{		
			NodeVO node = MazeModel.Instance().GetNode (pos.x, pos.y);
			
			GameStateModel.Instance().score.inc((int)((float)node.score * GameStateModel.Instance().timeBonus));
			
			if (GameStateModel.Instance().maxScore < GameStateModel.Instance().score) {
				GameStateModel.Instance().maxScore = GameStateModel.Instance().score;
			}
			
			GameStateModel.Instance().movesLeft.dec(1);
					
			if (GameStateModel.Instance().movesLeft < 1) {
				
				if (GameStateModel.Instance().maxScore > PlayerPrefs.GetInt ("highscore", 0))
					PlayerPrefs.SetInt ("highscore", GameStateModel.Instance().maxScore);
				
				AnalyticsWrapper.ReportGameLost (GameStateModel.Instance());
				
				SceneManager.LoadScene ("MenuScene");
				return;
			} 
				
			if (GameStateModel.Instance().state == GameStateModel.STATE_INITED || GameStateModel.Instance().state == GameStateModel.STATE_STUCK) {
				GameStateModel.Instance().state = GameStateModel.STATE_ACTIVATED;	
			}
			
			MazePaceNotifications.GAME_STATE_UPDATED.Dispatch (GameStateModel.Instance());
			
			if (node.HasFlag (NodeVO.SPECIALS_EXIT)) {
				OnExit (pos);
			} else {
				float moveTime = LevelModel.Instance ().moveTime;
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

				if (node.HasFlag (NodeVO.SPECIALS_HIDE_WALLS)) {
					MazePaceNotifications.TOGGLE_WALLS_VISIBILITY.Dispatch (false);
				}

				if (node.HasFlag (NodeVO.SPECIALS_SHOW_WALLS)) {
					MazePaceNotifications.TOGGLE_WALLS_VISIBILITY.Dispatch (true);
				}		

				if (!node.HasWall (directionIdx)) {
					MazePaceNotifications.NODE_REACHED.Dispatch (node, moveTime);
				} else {
					MazePaceNotifications.PLAYER_STUCK.Dispatch ();
				}
			}
		}

		public void OnExitClick ()
		{
			AnalyticsWrapper.ReportGameExit (GameStateModel.Instance());
			SceneManager.LoadScene ("MenuScene");
		}

		void OnExit (IntPointVO pos)
		{
			GameStateModel.Instance().state = GameStateModel.STATE_ENDED;
			GameStateModel.Instance().movesLeft.SetValue(GameStateModel.Instance().movesLeft, 0u, 0.5f);
			var avgScore = (LevelModel.Instance ().minScore + LevelModel.Instance ().maxScore) / 2;
			GameStateModel.Instance().score.SetValue(GameStateModel.Instance().score, (int)(GameStateModel.Instance().score + GameStateModel.Instance().movesLeft * GameStateModel.Instance().timeBonus * avgScore), 0.5f);
			GameStateModel.Instance().levelNumber++;
			MazePaceNotifications.GAME_STATE_UPDATED.Dispatch (GameStateModel.Instance());

			MazePaceNotifications.EXIT_REACHED.Dispatch ();
			Invoke ("Next", 0.6f);
		}

		void OnStuck ()
		{	
			GameStateModel.Instance().state = GameStateModel.STATE_STUCK;
			GameStateModel.Instance().score.SetValue(GameStateModel.Instance().score, 0, LevelModel.Instance ().scoreDrainTime);
		}


		public void OnDestroy ()
		{
			MazePaceNotifications.PLAYER_READY_TO_PROCEED.Remove (OnReadyToProceed);
			MazePaceNotifications.PLAYER_STUCK.Remove (OnStuck);
		}
	}
}