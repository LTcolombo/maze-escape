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

		void Start ()
		{
			CreateQueue ();
			                                      
			_renderer = GetComponent<SpriteRenderer> ();
			_audio = GetComponent<AudioSource> ();

			MazePaceNotifications.MAZE_RECREATED.Add (OnMazeRecreated);
			MazePaceNotifications.PROCEED_FROM_NODE.Add (Proceed);
			MazePaceNotifications.ROTATE_AT_NODE.Add (RotateAt);
			MazePaceNotifications.DIRECTION_UPDATED.Add (SetDirection);
			MazePaceNotifications.EXIT_REACHED.Add (OnExitReached);
			MazePaceNotifications.PLAYER_STUCK.Add (OnStuck);
		}

        void Update() {
            InvokeActions();
        }

		void OnMazeRecreated ()
		{
			transform.eulerAngles = new Vector3 (0, 0, -90 * PlayerModel.Instance ().directionIdx);
			transform.localPosition = new Vector3 (DifficultyModel.NODE_SIZE * PlayerModel.Instance ().cellPosition.x, 
				DifficultyModel.NODE_SIZE * PlayerModel.Instance ().cellPosition.y, 
				0);
			_renderer.enabled = true;
		}

		void Proceed (NodeVO node)
		{
			float moveTime = PlayerModel.Instance ().getMoveTime (node);
			if (moveTime > 0) {
				transform.DOMove (transform.position + new Vector3 (
					NodeVO.DIRECTIONS [PlayerModel.Instance ().directionIdx, 0] * DifficultyModel.NODE_SIZE, 
					NodeVO.DIRECTIONS [PlayerModel.Instance ().directionIdx, 1] * DifficultyModel.NODE_SIZE, 
					0
				), moveTime).OnComplete (OnMoveCompleted).SetEase (Ease.Linear);
			};
		}

		void OnMoveCompleted ()
		{
			MazePaceNotifications.NODE_REACHED.Dispatch ();
			transform.eulerAngles = new Vector3 (0, 0, -90 * PlayerModel.Instance ().directionIdx);
			InvokeCommand (MazePaceActions.TryMovePlayer);
		}

		void RotateAt (NodeVO node)
		{
			int rotateBy = 0;
			if (node.HasFlag (NodeVO.SPECIALS_ROTATOR_CW)) {
				rotateBy = 1;
			} else if (node.HasFlag (NodeVO.SPECIALS_ROTATOR_CCW)) {
				rotateBy = -1;
			} else {
				Debug.LogError ("RotateAt called on non rotation node");
			}
			if (rotateBy != 0) {
				transform.DORotate (transform.rotation.eulerAngles + new Vector3 (
					0, 0, rotateBy * -90), 0.4f).OnComplete (OnRotateCompleted);

				PlayerModel.Instance ().directionIdx += rotateBy;
			}				
		}

		void OnRotateCompleted ()
		{
			transform.eulerAngles = new Vector3 (0, 0, -90 * PlayerModel.Instance ().directionIdx);
			InvokeCommand (MazePaceActions.TryMovePlayer);
		}

		public void SetDirection (int value)
		{
			PlayerModel.Instance ().directionIdx = value;

			if (!DOTween.IsTweening (transform))
				OnMoveCompleted ();
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
			MazePaceNotifications.PROCEED_FROM_NODE.Remove (Proceed);
			MazePaceNotifications.ROTATE_AT_NODE.Remove (RotateAt);
			MazePaceNotifications.DIRECTION_UPDATED.Remove (SetDirection);
			MazePaceNotifications.EXIT_REACHED.Remove (OnExitReached);
			MazePaceNotifications.PLAYER_STUCK.Remove (OnStuck);
		}
	}
}
