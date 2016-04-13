using System;
using Model;
using Model.Data;
using UnityEngine;
using Utils;
using UnityEngine.SceneManagement;
using Notifications;

namespace Controller
{
	public class TryMovePlayer:Action
	{
		override public PrefromResult Perform(float delta){
			IntPointVO pos = PlayerModel.Instance ().cellPosition;
			int directionIdx = PlayerModel.Instance ().directionIdx;
			NodeVO node = MazeModel.Instance().GetNode (pos.x, pos.y);
			GameStateModel gameState = GameStateModel.Instance ();

			gameState.score.inc((int)((float)node.score * gameState.timeBonus));

			//incapsulate into gamestatemodel
			if (gameState.maxScore < gameState.score) {
				gameState.maxScore = gameState.score;
			}

			gameState.movesLeft.dec(1);

			if (gameState.movesLeft < 1) {

				//move stuff to command
				if (gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
					PlayerPrefs.SetInt ("highscore", gameState.maxScore);

				AnalyticsWrapper.ReportGameLost (gameState);

				SceneManager.LoadScene ("MenuScene");
				return PrefromResult.COMPLETED;
			} 

			if (gameState.state == GameStateModel.STATE_INITED || gameState.state == GameStateModel.STATE_STUCK) {
				gameState.state = GameStateModel.STATE_ACTIVATED;	
			}

			if (node.HasFlag (NodeVO.SPECIALS_EXIT)) {
				MazePaceNotifications.EXIT_REACHED.Dispatch ();
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
					gameState.state = GameStateModel.STATE_STUCK;
					gameState.score.SetValue(gameState.score, 0, LevelModel.Instance ().scoreDrainTime);
					MazePaceNotifications.PLAYER_STUCK.Dispatch ();
				}
			}

			MazePaceNotifications.GAME_STATE_UPDATED.Dispatch ();
			return PrefromResult.COMPLETED;
		}
	}
}

