using Utils;
using Model;
using UnityEngine;

namespace Controller
{
	public class ReportPause
	{
		public void Execute ()
		{
			AnalyticsWrapper.ReportGamePaused (GameStateModel.Instance());

			if (GameStateModel.Instance().maxScore > PlayerPrefs.GetInt ("highscore", 0))
				PlayerPrefs.SetInt ("highscore", GameStateModel.Instance().maxScore);
		}
	}
}

