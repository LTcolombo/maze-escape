using UnityEngine;
using UnityEngine.UI;
using Model;
using Notifications;

namespace View {
	public class TimeBonusMediator : MonoBehaviour {
		
		public string prefix = "TIME BONUS: ";
		public string format = "F1";
		
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

			RenderValue(state.timeBonus);
		}

		void Update(){
			GameModel state = GameModel.Instance ();
			if (state.state == GameModel.STATE_INITED) {
				RenderValue(state.timeBonus);
			}
		}

		void RenderValue(float value){
			_previousValue = value;
			_target.text = prefix + _previousValue.ToString (format);
		}

		void OnDestroy(){
			MazePaceNotifications.GAME_UPDATED.Remove(OnGameStateUpdated);
		}
	}
}