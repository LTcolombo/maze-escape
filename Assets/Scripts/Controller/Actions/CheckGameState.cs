using System;
using Model;
using Utils;
using UnityEngine.SceneManagement;
using UnityEngine;
using Notifications;

namespace Controller
{
	public class CheckGameState:Action
	{
		override public PrefromResult Perform(float delta){
			var game = GameModel.Instance ();
			if (game.GetIsLost ()) {
				if (game.maxScore > PlayerPrefs.GetInt ("highscore", 0))
					PlayerPrefs.SetInt ("highscore", game.maxScore);

				AnalyticsWrapper.ReportGameLost (game);
				LivesModel.Instance ().DecLives ();
				SceneManager.LoadScene ("MenuScene");
			} 

			return PrefromResult.PROCEED;
		}
	}
}

