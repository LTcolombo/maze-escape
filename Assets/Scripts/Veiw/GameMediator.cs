using UnityEngine;
using System;
using Notifications;
using Utils;
using Model;

namespace View
{
	public class GameMediator : ActionInvoker<MazePaceActions>
	{
		void Start()
        {
            InvokeAction (MazePaceActions.ResetModels);
			InvokeAction (MazePaceActions.CreateMaze);
			InvokeAction (MazePaceActions.ResetGameState);
			InvokeAction (MazePaceActions.CheckSwipe);
			InvokeAction (MazePaceActions.CheckGameState);

			MazePaceNotifications.EXIT_REACHED.Add (OnExitReached);
		}

        void OnExitReached ()
		{
			InvokeAction (MazePaceActions.ExitLevel);
			InvokeAction (MazePaceActions.AppendLevel);
			InvokeAction (MazePaceActions.WaitBeforeNextLevel);
			InvokeAction (MazePaceActions.CreateMaze);
			InvokeAction (MazePaceActions.ResetGameState);
		}

		void Update(){
            InvokeActions();
			if (GameModel.Instance().IsChanging()) {
				MazePaceNotifications.GAME_UPDATED.Dispatch ();
			}
		}

		void OnApplicationPause (bool paused)
		{
			AnalyticsWrapper.ReportGamePaused (DifficultyModel.Instance().number, GameModel.Instance());
			InvokeAction (MazePaceActions.AppendMaxScore);
		}

		public void OnDestroy ()
		{
			MazePaceNotifications.EXIT_REACHED.Remove (OnExitReached);
		}
	}
}

