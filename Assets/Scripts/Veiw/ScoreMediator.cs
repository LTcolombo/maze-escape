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
			_previousValue = -1;
			MazePaceNotifications.GAME_UPDATED.Add (OnGameStateUpdated);
		}

		void OnGameStateUpdated ()
		{
			GameModel game = GameModel.Instance ();
			if (game.state != _previousState) {
				if (game.state == GameModel.STATE_STUCK)
					_target.color = new Color (0.8f, 0.2f, 0.2f);
				else if (game.state == GameModel.STATE_ENDED)
					_target.color = new Color (0.2f, 0.8f, 0.2f);
				else
					_target.color = new Color (0.56f, 0.56f, 0.56f);
					
				_previousState = game.state;
			}
			
			if (_previousValue == game.score)
				return;
			
			RenderValue (game.score);
		}

		void Update(){
			if (_previousState == GameModel.STATE_STUCK) {
				RenderValue (GameModel.Instance().score);
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
