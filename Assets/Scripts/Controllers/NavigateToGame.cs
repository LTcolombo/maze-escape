using UnityEngine.SceneManagement;

namespace Controller
{
	public class NavigateToGame
	{
		public void Execute ()
		{
			SceneManager.LoadScene ("GameScene");
		}
	}
}

