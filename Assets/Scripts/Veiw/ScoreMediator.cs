using UnityEngine;
using UnityEngine.UI;
using Model;
using Notifications;

namespace View
{
	public class ScoreMediator : MonoBehaviour
	{
		
		public string prefix = "SCORE: ";
		public string format = "F0";
		private Text _target;
		private int _previousState;
		//move to state?
		private int _previousValue;
		private AudioSource _audio;

		void Awake ()
		{
			_target = GetComponent<Text> ();
			_audio = GetComponent<AudioSource> ();
			_previousValue = 0;
			MazePaceNotifications.GAME_UPDATED.Add (OnGameStateUpdated);
		}

		void OnGameStateUpdated ()
		{
			GameModel state = GameModel.Instance ();
			if (state.state != _previousState) {
				if (state.state == GameModel.STATE_STUCK)
					_target.color = new Color (0.8f, 0.2f, 0.2f);
				else if (state.state == GameModel.STATE_ENDED)
					_target.color = new Color (0.2f, 0.8f, 0.2f);
				else
					_target.color = new Color (0.56f, 0.56f, 0.56f);
					
				_previousState = state.state;
			}
			
			if (_previousValue == state.score)
				return;
			
			RenderValue (GameModel.Instance ().score);
		}

		void Update(){
			if (_previousState == GameModel.STATE_STUCK) {
				RenderValue (GameModel.Instance ().score);
			}
		}

		void RenderValue(int value){
			_target.text = prefix + value.ToString (format);

			if (_previousValue < value && _audio != null && !_audio.isPlaying)
				_audio.Play ();

			_previousValue = value;
		}

		void OnDestroy ()
		{
			MazePaceNotifications.GAME_UPDATED.Remove (OnGameStateUpdated);
		}
	}
}
