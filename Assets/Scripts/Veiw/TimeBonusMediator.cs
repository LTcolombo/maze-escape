using UnityEngine;
using UnityEngine.UI;
using Model;
using Notifications;
using DG.Tweening;

namespace View {
	public class TimeBonusMediator : MonoBehaviour {
		
		public string prefix = "{0}%";
		public string format = "F0";
		
		Text _target;
		float _previousValue;
		float _initialY;
		
		void Awake(){
			_target = GetComponent<Text> ();
			_previousValue = 0;
			_initialY = transform.localPosition.y;
			MazePaceNotifications.MAZE_RECREATED.Add(OnMazeRecreated);
			MazePaceNotifications.GAME_UPDATED.Add(OnGameStateUpdated);
			MazePaceNotifications.EXIT_REACHED.Add(OnExitReached);
		}
			
		void OnMazeRecreated(){
			transform.DOLocalMoveY (_initialY, 0.4f);
		}

		void OnGameStateUpdated () {
			GameModel state = GameModel.Instance ();

			if (_previousValue == state.timeBonus)
				return;

			UpdateValue(state.timeBonus);
		}

		void OnExitReached(){
			transform.DOLocalMoveY (0, 0.4f);
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
			MazePaceNotifications.MAZE_RECREATED.Remove(OnMazeRecreated);
			MazePaceNotifications.GAME_UPDATED.Remove(OnGameStateUpdated);
			MazePaceNotifications.EXIT_REACHED.Remove(OnExitReached);
		}
	}
}