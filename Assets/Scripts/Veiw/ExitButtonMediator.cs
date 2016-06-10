using Utils;
using Model;

namespace View
{
	public class ExitButtonMediator : ActionInvoker<MazePaceActions>
	{
        void Start()
        {
            CreateQueue();
        }

        public void OnExitClick(){
			AnalyticsWrapper.ReportReturnToMenu (GameModel.Instance());
			InvokeAction(MazePaceActions.NavigateToMenu);
		}

        void Update()
        {
            InvokeActions();
        }
    }
}

