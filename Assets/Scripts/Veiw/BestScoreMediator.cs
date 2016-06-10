using UnityEngine.UI;
using UnityEngine;
using Model;

namespace View
{
	public class BestScoreMediator : MonoBehaviour
	{
		Text _target;
		int _oldValue;

		protected void Start ()
		{
			_target = GetComponent<Text> ();
			if (_target == null) {
				Debug.Log ("No text component found!");
			}
		}

		void Update(){
			var game = GameModel.Instance ();
			if (_oldValue != game.maxScore) {
				_oldValue = game.maxScore;
				_target.text = "BEST SCORE: " + (int)game.maxScore;
			}
		}
	}
}

