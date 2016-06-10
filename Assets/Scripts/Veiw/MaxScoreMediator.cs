using UnityEngine;
using UnityEngine.UI;
using Model;
using Notifications;

namespace View {
	public class MaxScoreMediator : MonoBehaviour {
		
		public string prefix = "MAX SCORE: ";
		public string format = "F0";
		
		Text _target;
		int _previousValue;
		
		void Start(){
			_target = GetComponent<Text> ();
			_previousValue = -1;
		}

		void Update () {
			GameModel game = GameModel.Instance ();
			int newValue = game.maxScore;
			if (_previousValue == newValue)
				return;
				
			_previousValue = newValue;
			_target.text = prefix + newValue;
		}
		
		void OnDestroy(){
		}
	}
}
