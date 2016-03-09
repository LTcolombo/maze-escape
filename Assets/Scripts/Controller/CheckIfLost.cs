using System;
using Model;
using Utils;
using UnityEngine.SceneManagement;

namespace Controller
{
	public class CheckIfLost
	{
		void Execute(){
			var gameStateModel = GameStateModel.Instance ();
			if (gameStateModel.state == GameStateModel.STATE_STUCK) {
				if (gameStateModel.score <= 0) {
					AnalyticsWrapper.ReportGameLost (gameStateModel);
					SceneManager.LoadScene ("MenuScene");
				} 
			}
		}
	}
}

