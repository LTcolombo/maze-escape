﻿using System;

namespace Controller
{
	public class ActionFactory<T>
	{
		public Action Create (T id)
		{

			MazePaceActions mpid = (MazePaceActions)(object) id;

			{
				switch (mpid) {
				case (MazePaceActions.AppendLevel):
					return new AppendLevel ();
				case(MazePaceActions.AppendMaxScore):
					return new AppendMaxScore ();
				case(MazePaceActions.CheckGameState):
					return new CheckGameState ();
				case(MazePaceActions.CheckSwipe):
					return new CheckSwipe ();
				case(MazePaceActions.CreateMaze):
					return new CreateMaze ();
				case(MazePaceActions.ExitLevel):
					return new ExitLevel ();
				case(MazePaceActions.NavigateToGame):
					return new NavigateToGame ();
				case(MazePaceActions.NavigateToMenu):
					return new NavigateToMenu ();
				case(MazePaceActions.ResetGameState):
					return new ResetGameState ();
				case(MazePaceActions.ResetModels):
					return new ResetModels ();
				case(MazePaceActions.TryMovePlayer):
					return new TryMovePlayer ();
				case(MazePaceActions.WaitBeforeNextLevel):
					return new WaitBeforeNextLevel ();
				}
				return null;
			}
		}
	}
}

