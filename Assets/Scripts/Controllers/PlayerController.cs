using UnityEngine;
using System.Collections;
using DG.Tweening;
using Model;
using Model.Data;
using Notifications;

public delegate void PlayerStepComplete ();

namespace View {
	public class PlayerController : MonoBehaviour
	{
		private Vector2 _touchStartPoint;
		private SpriteRenderer _renderer;
		private AudioSource _audio;

		// Use this for initialization
		void Start ()
		{
			transform.eulerAngles = new Vector3 (0, 0, -90 * PlayerModel.Instance().directionIdx);
			transform.localPosition = new Vector3 (LevelModel.NODE_SIZE * PlayerModel.Instance().cellPosition.x, 
				LevelModel.NODE_SIZE * PlayerModel.Instance().cellPosition.y, 
			                                      0);
			                                      
			_renderer = GetComponent<SpriteRenderer> ();
			_audio = GetComponent<AudioSource> ();

			MazePaceNotifications.MAZE_RECREATED.Add (onMazeDataUpdated);
			MazePaceNotifications.NODE_REACHED.Add (Proceed);
			MazePaceNotifications.EXIT_REACHED.Add (OnExitReached);
			MazePaceNotifications.PLAYER_STUCK.Add (OnStuck);
		}
		
		void onMazeDataUpdated (MazeModel data)
		{
			_renderer.enabled = true;
		}
		
		void OnReadyToProceed ()
		{
			DOTween.CompleteAll ();
			transform.eulerAngles = new Vector3 (0, 0, -90 * PlayerModel.Instance().directionIdx);
			MazePaceNotifications.PLAYER_READY_TO_PROCEED.Dispatch (PlayerModel.Instance().cellPosition, PlayerModel.Instance().directionIdx);
		}

		void Proceed (NodeVO node, float moveTime)
		{
			if (moveTime > 0) {
				transform.DOMove (transform.position + new Vector3 (
					NodeVO.DIRECTIONS [ PlayerModel.Instance().directionIdx, 0] * LevelModel.NODE_SIZE, 
					NodeVO.DIRECTIONS [ PlayerModel.Instance().directionIdx, 1] * LevelModel.NODE_SIZE, 
					0
				), moveTime).OnComplete (OnReadyToProceed).SetEase (Ease.Linear);

				PlayerModel.Instance().cellPosition.x += NodeVO.DIRECTIONS [ PlayerModel.Instance().directionIdx, 0];
				PlayerModel.Instance().cellPosition.y += NodeVO.DIRECTIONS [ PlayerModel.Instance().directionIdx, 1];
			};
		}

		void OnExitReached(){
			_renderer.enabled = false;
		}

		void OnStuck(){
			_audio.Play ();
		}

		// Update is called once per frame
		void Update ()
		{
			if (!_renderer.isVisible)//todo make it not binded to renderer
				return;
			//keyboard input
			
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				SetDirection (NodeVO.DIRECTION_UP_IDX);
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				SetDirection (NodeVO.DIRECTION_RIGHT_IDX);
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				SetDirection (NodeVO.DIRECTION_DOWN_IDX);
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				SetDirection (NodeVO.DIRECTION_LEFT_IDX);
			} 
					
			if (Input.touchCount > 0) {
				for (int i = 0; i < Input.touchCount; i++) {
					Touch touch = Input.GetTouch (i);
				
					if (touch.phase == TouchPhase.Began) {
						_touchStartPoint = touch.position;
					} else if (touch.phase == TouchPhase.Ended) {
									
						Vector2 delta = touch.position - _touchStartPoint;
						if (delta.magnitude == 0)
							continue;
						
						if (Mathf.Abs (delta.x) > Mathf.Abs (delta.y)) {
							if (delta.x > 0) 
								SetDirection (NodeVO.DIRECTION_RIGHT_IDX);
							else
								SetDirection (NodeVO.DIRECTION_LEFT_IDX);
						} else {
							if (delta.y > 0) 
								SetDirection (NodeVO.DIRECTION_UP_IDX);
							else
								SetDirection (NodeVO.DIRECTION_DOWN_IDX);
						}
					}
				}
			}
		}
		
		public void SetDirection (int value)
		{
			PlayerModel.Instance().directionIdx = value;
					
			if (!DOTween.IsTweening (transform)) 
				OnReadyToProceed ();

			MazePaceNotifications.PLAYER_DIRECTION_UPDATED.Dispatch (PlayerModel.Instance().directionIdx);
		}

		public void OnDestroy(){
			MazePaceNotifications.MAZE_RECREATED.Remove (onMazeDataUpdated);
			MazePaceNotifications.NODE_REACHED.Remove (Proceed);
			MazePaceNotifications.EXIT_REACHED.Remove (OnExitReached);
			MazePaceNotifications.PLAYER_STUCK.Remove (OnStuck);
		}
	}
}
