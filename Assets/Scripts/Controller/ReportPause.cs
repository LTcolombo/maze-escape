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
			//SaveMaxScore ();
		}
	}
}

