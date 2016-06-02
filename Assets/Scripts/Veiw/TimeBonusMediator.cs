using UnityEngine;
using UnityEngine.UI;
using Model;
using Notifications;

namespace View {
	public class TimeBonusMediator : MonoBehaviour {
		
		public string prefix = "{0}%";
		public string format = "F0";
		
		Text _target;
		float _previousValue;
		
		void Awake(){
			_target = GetComponent<Text> ();
			_previousValue = 0;
			MazePaceNotifications.GAME_UPDATED.Add(OnGameStateUpdated);
		}

		void OnGameStateUpdated () {
			GameModel state = GameModel.Instance ();

			if (_previousValue == state.timeBonus)
				return;

			UpdateValue(state.timeBonus);
		}

		void Update(){
			GameModel state = GameModel.Instance ();
			if (state.state == GameModel.STATE_INITED) {
				UpdateValue(state.timeBonus);
			}
		}

		void UpdateValue(float value){
			_previousValue = value;
			_target.enabled = true;
			_target.text = "+" + ((value - 1) * 100).ToString (format) + "%";
		}

		void OnDestroy(){
			MazePaceNotifications.GAME_UPDATED.Remove(OnGameStateUpdated);
		}
	}
}