﻿using Model;
using Model.Decorators;
using Notifications;


namespace Controller
{
	public class CreateMaze:Action
	{
		public override PrefromResult Perform (float delta)
		{

			var difficultyModel = DifficultyModel.Instance ();
			var cellPosition = PlayerModel.Instance ().cellPosition;
			MazeModel.Instance ().Recreate (difficultyModel.size, cellPosition.x, cellPosition.y);
			ExitDecorator.Apply (MazeModel.Instance ());
			ScoreDecorator.Apply (MazeModel.Instance ());
			HiderDecorator.Apply (MazeModel.Instance ());
			SpeedUpDecorator.Apply (MazeModel.Instance ());
			RotatorDecorator.Apply (MazeModel.Instance ());

			MazePaceNotifications.MAZE_RECREATED.Dispatch ();

			return PrefromResult.COMPLETED;
		}
	}
}

