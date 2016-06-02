using UnityEngine;
using Model;

namespace View
{
	public class VibrationMediator:MonoBehaviour
	{
		bool _vibrating;

		void Start(){
			enabled = !Application.isEditor;
		}

		void Update ()
		{
			if (GameModel.Instance ().state == GameModel.STATE_STUCK) {
				if (!_vibrating) {
					_vibrating = true;
					Handheld.Vibrate ();
				}
			} else {
				_vibrating = false;
			}
		}
	}
}

