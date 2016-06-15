using UnityEngine;
using UnityEngine.UI;
using Model;
using Notifications;
using System;
using DG.Tweening;

namespace View
{
	public class MovesMediator : MonoBehaviour
	{
		public float alpha = 0.2f;
		public float sizePerCell = 32;
		public int maxVisibleMoves = 99;

		Text _target;
		AudioSource _audio;

		uint _previousValue;
		uint _movesForLevel;

		Vector3 _initialScale;

		void Awake ()
		{
			_target = GetComponent<Text> ();
			_audio = GetComponent<AudioSource> ();
			_previousValue = 0;
			MazePaceNotifications.MAZE_RECREATED.Add (OnMazeRecreated);
			MazePaceNotifications.GAME_UPDATED.Add (OnGameStateUpdated);
			MazePaceNotifications.EXIT_REACHED.Add (OnExitReached);

			_initialScale = transform.localScale;
		}

		void OnMazeRecreated ()
		{
			_target.fontSize = Mathf.CeilToInt (sizePerCell * MazeModel.Instance ().size);
			transform.DOScale (_initialScale, 0.4f);
			transform.DOLocalMoveY (0, 0.4f);
		}

		void OnGameStateUpdated ()
		{
			GameModel game = GameModel.Instance ();
			if (game.movesLeft == _previousValue)
				return;

			if (game.state == GameModel.STATE_INITED) {
				_movesForLevel = (uint)Math.Min (maxVisibleMoves, game.movesLeft);
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
			_target.text = ((int)game.movesLeft).ToString();

			_previousValue = game.movesLeft;
		}

		void OnExitReached ()
		{
			_target.color = new Color (0.4f, 0.8f, 0.4f, 1);
			transform.DOScale (sizePerCell/_target.fontSize, 0.4f);
			transform.DOLocalMoveY (-sizePerCell, 0.4f);
		}

		void OnDestroy ()
		{
			MazePaceNotifications.MAZE_RECREATED.Remove (OnMazeRecreated);
			MazePaceNotifications.GAME_UPDATED.Remove (OnGameStateUpdated);
			MazePaceNotifications.EXIT_REACHED.Remove (OnExitReached);
		}
	}
}
