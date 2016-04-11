using UnityEngine;
using System;

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
		}
	}
}

