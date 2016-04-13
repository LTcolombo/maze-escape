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
			MazePaceNotifications.GAME_STATE_UPDATED.Add(OnGameStateUpdated);
		}

		void OnGameStateUpdated () {
			GameStateModel state = GameStateModel.Instance ();

			if (_previousValue == state.timeBonus)
				return;

			RenderValue(state.timeBonus);
		}

		void Update(){
			GameStateModel state = GameStateModel.Instance ();
			if (state.state == GameStateModel.STATE_INITED) {
				RenderValue(state.timeBonus);
			}
		}

		void RenderValue(float value){
			_previousValue = value;
			_target.text = prefix + _previousValue.ToString (format);
		}

		void OnDestroy(){
			MazePaceNotifications.GAME_STATE_UPDATED.Remove(OnGameStateUpdated);
		}
	}
}