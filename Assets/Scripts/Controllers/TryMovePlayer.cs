using System;
using Model;
using Model.Data;
using UnityEngine;
using Utils;
using UnityEngine.SceneManagement;
using Notifications;

namespace Controller
{
	public class TryMovePlayer
	{
		void Execute(){
			IntPointVO pos = PlayerModel.Instance ().cellPosition;

			int directionIdx = PlayerModel.Instance ().directionIdx;

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

			if (node.HasFlag (NodeVO.SPECIALS_EXIT)) {
				//OnExit (pos);
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
					GameStateModel.Instance().state = GameStateModel.STATE_STUCK;
					GameStateModel.Instance().score.SetValue(GameStateModel.Instance().score, 0, LevelModel.Instance ().scoreDrainTime);
					MazePaceNotifications.PLAYER_STUCK.Dispatch ();
				}
			}
		}
	}
}

