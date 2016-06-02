using UnityEngine.SceneManagement;
using Utils;

namespace Controller
{
	public class NavigateToGame:Action
	{
		override public PrefromResult Perform (float delta)
		{
			SceneManager.LoadScene ("GameScene");
			return PrefromResult.COMPLETED;
		}
	}
}

