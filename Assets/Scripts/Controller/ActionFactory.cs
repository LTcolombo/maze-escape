using System;

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
				case(MazePaceActions.CheckIfLost):
					return new CheckIfLost ();
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
				case(MazePaceActions.ReportPause):
					return new ReportPause ();
				case(MazePaceActions.ResetGameState):
					return new ResetGameState ();
				case(MazePaceActions.RetrieveLevel):
					return new RetrieveLevel ();
				case(MazePaceActions.TryMovePlayer):
					return new TryMovePlayer ();
				}
				return null;
			}
		}
	}
}

