using UnityEngine;
using Model;

namespace View
{
	public class VibrationMediator:MonoBehaviour
	{
		void Update ()
		{
			if (GameModel.Instance ().state == GameModel.STATE_STUCK)
				Handheld.Vibrate ();
		}
	}
}

