using System;
using Model;
using Utils;
using UnityEngine.SceneManagement;
using UnityEngine;
using Notifications;

namespace Controller
{
	public class CheckIfLost:Action
	{
		override public PrefromResult Perform(float delta){
			var gameState = GameModel.Instance ();
			if (gameState.GetIsLost ()) {
				if (gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
					PlayerPrefs.SetInt ("highscore", gameState.maxScore);

				AnalyticsWrapper.ReportGameLost (gameState);
				LivesModel.Instance().DecLives ();
				SceneManager.LoadScene ("MenuScene");
			}

			return PrefromResult.PROCEED;
		}
	}
}

