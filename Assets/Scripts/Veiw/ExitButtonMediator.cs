using Utils;
using Model;

namespace View
{
	public class ExitButtonMediator : ActionInvoker<MazePaceActions>
	{
		public void OnExitClick(){
			AnalyticsWrapper.ReportReturnToMenu (GameModel.Instance());
			InvokeCommand(MazePaceActions.NavigateToMenu);
		}
	}
}

