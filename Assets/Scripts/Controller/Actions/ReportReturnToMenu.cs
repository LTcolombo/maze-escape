using System;
using Model;
using Model.Decorators;
using Notifications;
using Utils;

namespace Controller
{
	public class ReportReturnToMenu:Action
	{
		override public PrefromResult Perform (float delta)
		{
			AnalyticsWrapper.ReportReturnToMenu (GameStateModel.Instance());

			return PrefromResult.COMPLETED;
		}
	}
}

