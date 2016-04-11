using Utils;
using UnityEngine.SceneManagement;
using Model;

namespace Controller
{
	public class NavigateToMenu:Action
	{
		override public PrefromResult Perform (float delta)
		{
			SceneManager.LoadScene ("MenuScene");

			return PrefromResult.COMPLETED;
		}
	}
}

