using Utils;
using Model;
using UnityEngine;

namespace Controller
{
	public class ReportPause:Action
	{
		override public PrefromResult Perform (float delta)
		{
			AnalyticsWrapper.ReportGamePaused (GameStateModel.Instance());

			return PrefromResult.COMPLETED;
		}
	}
}

