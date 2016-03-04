using UnityEngine;
using UnityEngine.UI;
using Model;
using Notifications;

namespace Views {
	public class TimeBonusMediator : MonoBehaviour {
		
		public string prefix = "TIME BONUS: ";
		public string format = "F1";
		
		Text _target;
		float _previousValue;
		
		void Awake(){
			_target = GetComponent<Text> ();
			_previousValue = 0;
			MazePaceNotifications.GAME_STATE_UPDATED.Add(OnGameStateUpdated);
		}
		
		// Update is called once per frame
		void OnGameStateUpdated (GameStateModel state) {
			
			if (_previousValue == state.timeBonus)
				return;
				
			_previousValue = state.timeBonus;
			_target.text = prefix + state.timeBonus.ToString (format);
		}
		
		void OnDestroy(){
			MazePaceNotifications.GAME_STATE_UPDATED.Remove(OnGameStateUpdated);
		}
	}
}