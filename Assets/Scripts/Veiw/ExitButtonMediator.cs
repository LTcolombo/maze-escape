using Utils;
using Model;

namespace View
{
	public class ExitButtonMediator : ActionInvoker<MazePaceActions>
	{
        public void OnExitClick(){
			AnalyticsWrapper.ReportReturnToMenu (DifficultyModel.Instance().number, GameModel.Instance());
			InvokeAction(MazePaceActions.NavigateToMenu);
		}

        void Update()
        {
            InvokeActions();
        }
    }
}

