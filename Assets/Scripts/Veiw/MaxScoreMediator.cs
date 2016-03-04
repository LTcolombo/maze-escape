using UnityEngine;
using UnityEngine.UI;
using Model;
using Notifications;

namespace Views {
	public class MaxScoreMediator : MonoBehaviour {
		
		public string prefix = "MAX SCORE: ";
		public string format = "F0";
		
		Text _target;
		int _previousValue;
		
		void Awake(){
			_target = GetComponent<Text> ();
			_previousValue = 0;
			MazePaceNotifications.GAME_STATE_UPDATED.Add(OnGameStateUpdated);
		}
		
		// Update is called once per frame
		void OnGameStateUpdated (GameStateModel state) {
			if (_previousValue == state.maxScore)
				return;
				
			_previousValue = state.maxScore;
			_target.text = prefix + state.maxScore.ToString (format);
		}
		
		void OnDestroy(){
			MazePaceNotifications.GAME_STATE_UPDATED.Remove(OnGameStateUpdated);
		}
	}
}
