using UnityEngine;
using UnityEngine.UI;
using Model;
using Notifications;
using System;

namespace View
{
	public class MovesMediator : MonoBehaviour
	{
		public string template = "{0}%";
		public float alpha = 0.2f;
		public float sizePerCell = 0.1f;
		public int maxVisibleMoves = 99;
		int _relativeSize;

		Text _target;
		AudioSource _audio;

		uint _previousValue;
		uint _movesForLevel;

		void Awake ()
		{
			_target = GetComponent<Text> ();
			_audio = GetComponent<AudioSource> ();
			_previousValue = 0;
			MazePaceNotifications.MAZE_RECREATED.Add (OnMazeRecreated);
			MazePaceNotifications.GAME_UPDATED.Add (OnGameStateUpdated);
			MazePaceNotifications.EXIT_REACHED.Add (OnExitReached);
		}

		void OnMazeRecreated ()
		{
			_target.fontSize = Mathf.CeilToInt (sizePerCell * MazeModel.Instance ().size);
		}

		void OnGameStateUpdated ()
		{
			GameModel game = GameModel.Instance ();
			if (game.movesLeft == _previousValue)
				return;

			if (game.state == GameModel.STATE_INITED) {
				_movesForLevel = (uint)Math.Min(maxVisibleMoves, game.movesLeft);
			}

			if (game.movesLeft > _movesForLevel) {
				_target.enabled = false;
				return;
			} else {
				_target.enabled = true;
			}
			
			if (game.state == GameModel.STATE_MOVING && !_audio.isPlaying)
				_audio.Play ();

			if (game.state != GameModel.STATE_ENDED) {
				float percentage = (float)(game.movesLeft) / _movesForLevel;
				_target.color = new Color (1, percentage, percentage, alpha);
			}
			_target.text = String.Format (template, (int)game.movesLeft);

			_previousValue = game.movesLeft;
		}

		void OnExitReached(){
			_target.color = new Color (0, 1, 0, alpha);
		}

		void OnDestroy ()
		{
			MazePaceNotifications.MAZE_RECREATED.Remove (OnMazeRecreated);
			MazePaceNotifications.GAME_UPDATED.Remove (OnGameStateUpdated);
			MazePaceNotifications.EXIT_REACHED.Remove (OnExitReached);
		}
	}
}
