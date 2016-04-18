using UnityEngine;
using System.Collections;
using DG.Tweening;
using Model;
using Model.Data;
using Notifications;

namespace View
{
	public class PlayerMediator : ActionInvoker<MazePaceActions>
	{
		private SpriteRenderer _renderer;
		private AudioSource _audio;

		override protected void Start(){
			base.Start ();
			                                      
			_renderer = GetComponent<SpriteRenderer> ();
			_audio = GetComponent<AudioSource> ();

			MazePaceNotifications.MAZE_RECREATED.Add (OnMazeRecreated);
			MazePaceNotifications.NODE_REACHED.Add (Proceed);
			MazePaceNotifications.SET_PLAYER_DIRECTION.Add (SetDirection);
			MazePaceNotifications.EXIT_REACHED.Add (OnExitReached);
			MazePaceNotifications.PLAYER_STUCK.Add (OnStuck);
		}

		void OnMazeRecreated (MazeModel data)
		{
			transform.eulerAngles = new Vector3 (0, 0, -90 * PlayerModel.Instance ().directionIdx);
			transform.localPosition = new Vector3 (LevelModel.NODE_SIZE * PlayerModel.Instance ().cellPosition.x, 
				LevelModel.NODE_SIZE * PlayerModel.Instance ().cellPosition.y, 
				0);
			_renderer.enabled = true;
		}

		void Proceed (NodeVO node, float moveTime)
		{
			if (moveTime > 0) {
				transform.DOMove (transform.position + new Vector3 (
					NodeVO.DIRECTIONS [PlayerModel.Instance ().directionIdx, 0] * LevelModel.NODE_SIZE, 
					NodeVO.DIRECTIONS [PlayerModel.Instance ().directionIdx, 1] * LevelModel.NODE_SIZE, 
					0
				), moveTime).OnComplete (OnReadyToProceed).SetEase (Ease.Linear);

				PlayerModel.Instance ().cellPosition.x += NodeVO.DIRECTIONS [PlayerModel.Instance ().directionIdx, 0];
				PlayerModel.Instance ().cellPosition.y += NodeVO.DIRECTIONS [PlayerModel.Instance ().directionIdx, 1];
			};
		}

		public void SetDirection (int value)
		{
			PlayerModel.Instance ().directionIdx = value;

			if (!DOTween.IsTweening (transform))
				OnReadyToProceed ();
		}

		void OnReadyToProceed ()
		{
			DOTween.CompleteAll ();
			transform.eulerAngles = new Vector3 (0, 0, -90 * PlayerModel.Instance ().directionIdx);

			InvokeCommand (MazePaceActions.TryMovePlayer);
		}

		void OnExitReached ()
		{
			_renderer.enabled = false;
		}

		void OnStuck ()
		{
			_audio.Play ();
		}

		public void OnDestroy ()
		{
			MazePaceNotifications.MAZE_RECREATED.Remove (OnMazeRecreated);
			MazePaceNotifications.NODE_REACHED.Remove (Proceed);
			MazePaceNotifications.SET_PLAYER_DIRECTION.Remove (SetDirection);
			MazePaceNotifications.EXIT_REACHED.Remove (OnExitReached);
			MazePaceNotifications.PLAYER_STUCK.Remove (OnStuck);
		}
	}
}
