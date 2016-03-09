using System;
using Model;
using UnityEngine;
using Model.Decorators;
using Notifications;

namespace Controller
{
	public class StartLevel
	{
		void Execute(){
			//SaveLevel ();

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
	}
}

