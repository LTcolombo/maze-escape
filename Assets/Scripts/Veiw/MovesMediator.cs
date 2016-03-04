using UnityEngine;
using UnityEngine.UI;
using Model;
using Notifications;

namespace View {
	public class MovesMediator : MonoBehaviour
	{
		public string prefix = "MOVES: ";
		public string format = "F0";
		Text _target;
		AudioSource _audio;
		uint _previousValue;

		const int CRITICAL_MOVES = 5;
		
		void Awake ()
		{
			_target = GetComponent<Text> ();
			_audio = GetComponent<AudioSource> ();
			_previousValue = 0;
			MazePaceNotifications.GAME_STATE_UPDATED.Add(OnGameStateUpdated);
		}

		void OnGameStateUpdated (GameStateModel state)
		{
			if (state.movesLeft == _previousValue)
				return;
			
			if (!_audio.isPlaying)
				_audio.Play ();
				
			_previousValue = state.movesLeft;
				
			
			if (_previousValue < CRITICAL_MOVES && state.state != GameStateModel.STATE_ENDED)
				_target.color = new Color (0.8f, 0.2f, 0.2f);
			else
				_target.color = new Color (0.56f, 0.56f, 0.56f);
				
			uint movesLeft = state.movesLeft;
			_target.text = prefix + movesLeft.ToString (format);
		}
		
		void OnDestroy(){
			MazePaceNotifications.GAME_STATE_UPDATED.Remove(OnGameStateUpdated);
		}
	}
}
