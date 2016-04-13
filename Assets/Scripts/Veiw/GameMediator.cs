using UnityEngine;
using System;
using Notifications;

namespace View
{
	public class GameMediator : ActionInvoker<MazePaceActions>
	{
		override protected void Start(){
			base.Start ();
			InvokeCommand (MazePaceActions.RetrieveLevel);
			InvokeCommand (MazePaceActions.CreateMaze);
			InvokeCommand (MazePaceActions.ResetGameState);
			InvokeCommand (MazePaceActions.CheckSwipe);
			InvokeCommand (MazePaceActions.CheckIfLost);

			MazePaceNotifications.EXIT_REACHED.Add (OnExitReached);
		}

		void OnExitReached ()
		{
			InvokeCommand (MazePaceActions.ExitLevel);
			InvokeCommand (MazePaceActions.AppendLevel);
			InvokeCommand (MazePaceActions.WaitBeforeNextLevel);
			InvokeCommand (MazePaceActions.CreateMaze);
			InvokeCommand (MazePaceActions.ResetGameState);
		}

		public void OnDestroy ()
		{
			MazePaceNotifications.EXIT_REACHED.Remove (OnExitReached);
		}
	}
}

