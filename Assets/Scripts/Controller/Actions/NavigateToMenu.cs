using Utils;
using UnityEngine.SceneManagement;
using Model;

namespace Controller
{
	public class NavigateToMenu:Action
	{
		override public PrefromResult Perform (float delta)
		{
			LivesModel.Instance().DecLives ();
			AnalyticsWrapper.ReportReturnToMenu (DifficultyModel.Instance().number, GameModel.Instance());
			SceneManager.LoadScene ("MenuScene");
			return PrefromResult.COMPLETED;
		}
	}
}

