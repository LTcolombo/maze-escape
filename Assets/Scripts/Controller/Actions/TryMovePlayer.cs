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
		override public PrefromResult Perform (float delta)
		{
			var player = PlayerModel.Instance ();
			IntPointVO pos = player.cellPosition;
			int directionIdx = player.directionIdx;
			NodeVO node = MazeModel.Instance ().GetNode (pos.x, pos.y);
			GameModel game = GameModel.Instance ();

			game.AddScore ((int)((float)node.score * game.timeBonus));
			node.score = 0;

			game.movesLeft.Dec (1);

			if (game.state == GameModel.STATE_INITED || game.state == GameModel.STATE_STUCK) {
				game.state = GameModel.STATE_MOVING;	
				game.score.Freeze();
				game.timeBonus.Freeze();
			}

			if (node.HasFlag (NodeVO.SPECIALS_EXIT)) {
				game.state = GameModel.STATE_ENDED;
				MazePaceNotifications.EXIT_REACHED.Dispatch ();
			} else {
				if (node.HasFlag (NodeVO.SPECIALS_HIDE_WALLS)) {
					MazePaceNotifications.TOGGLE_WALLS_VISIBILITY.Dispatch (false);
				}
				if (node.HasFlag (NodeVO.SPECIALS_SHOW_WALLS)) {
					MazePaceNotifications.TOGGLE_WALLS_VISIBILITY.Dispatch (true);
				}
				bool shouldRotate = node.HasWall (directionIdx) || player.moved;
				if (node.HasFlag (NodeVO.SPECIALS_ROTATOR_CW | NodeVO.SPECIALS_ROTATOR_CCW) && shouldRotate) {
					MazePaceNotifications.ROTATE_AT_NODE.Dispatch (node);
					player.moved = false;
				} else if (!node.HasWall (directionIdx)) {
					player.cellPosition.x += NodeVO.DIRECTIONS [player.directionIdx, 0];
					player.cellPosition.y += NodeVO.DIRECTIONS [player.directionIdx, 1];
					player.moved = true;
					MazePaceNotifications.PROCEED_FROM_NODE.Dispatch (node);
				} else {
					game.state = GameModel.STATE_STUCK;
					game.score.SetValue (game.score, 0, DifficultyModel.Instance ().scoreDrainTime);
					MazePaceNotifications.PLAYER_STUCK.Dispatch ();
				}
			}

			MazePaceNotifications.GAME_UPDATED.Dispatch ();
			return PrefromResult.COMPLETED;
		}
	}
}