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
            CreateQueue();
            InvokeCommand (MazePaceActions.ResetModels);
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

		void Update(){
            InvokeActions();
            if (GameModel.Instance().IsChanging()) {
				MazePaceNotifications.GAME_UPDATED.Dispatch ();
			}
		}

		void OnApplicationPause (bool paused)
		{
			AnalyticsWrapper.ReportGamePaused (GameModel.Instance());

			if (GameModel.Instance().maxScore > PlayerPrefs.GetInt ("highscore", 0))
				PlayerPrefs.SetInt ("highscore", GameModel.Instance().maxScore);
		}

		public void OnDestroy ()
		{
			MazePaceNotifications.EXIT_REACHED.Remove (OnExitReached);
		}
	}
}

