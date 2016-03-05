using Utils;
using UnityEngine.SceneManagement;
using Model;

namespace Controller
{
	public class NavigateToMenu
	{
		public void Execute ()
		{
			AnalyticsWrapper.ReportGameExit (GameStateModel.Instance());
			SceneManager.LoadScene ("MenuScene");
		}
	}
}

