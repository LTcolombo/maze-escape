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
			MazePaceNotifications.GAME_UPDATED.Add (OnGameStateUpdated);
		}

		// Update is called once per frame
		void OnGameStateUpdated () {
			GameModel game = GameModel.Instance ();
			if (_previousValue == game.maxScore)
				return;
				
			_previousValue = game.maxScore;
			_target.text = prefix + game.maxScore.ToString (format);
		}
		
		void OnDestroy(){
			MazePaceNotifications.GAME_UPDATED.Remove(OnGameStateUpdated);
		}
	}
}
