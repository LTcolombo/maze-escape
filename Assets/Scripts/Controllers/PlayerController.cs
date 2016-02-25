using UnityEngine;
using System.Collections;
using DG.Tweening;
using Models;
using Models.Data;
using Notifications;

public delegate void PlayerStepComplete ();

namespace Controllers {
	public class PlayerController : MonoBehaviour
	{
		public IntPoint cellPosition { get { return _cellPosition; } }

		private IntPoint _cellPosition;
		private int _directionIdx;
		private Vector2 _touchStartPoint;
		private SpriteRenderer _renderer;
		private AudioSource _audio;

		// Use this for initialization
		void Start ()
		{
			_cellPosition = new IntPoint (0, 0);

			_directionIdx = NodeModel.DIRECTION_UP_IDX;
			transform.eulerAngles = new Vector3 (0, 0, -90 * _directionIdx);
			transform.localPosition = new Vector3 (MazeController.NODE_SIZE * _cellPosition.x, 
			                                      MazeController.NODE_SIZE * _cellPosition.y, 
			                                      0);
			                                      
			_renderer = GetComponent<SpriteRenderer> ();
			_audio = GetComponent<AudioSource> ();

			NotificationManager.MAZE_DATA_UPDATED.Add (onMazeDataUpdated);
			NotificationManager.PROCEED.Add (Proceed);
			NotificationManager.EXIT_REACHED.Add (OnExit);
			NotificationManager.PLAYER_STUCK.Add (OnStuck);
		}
		
		void onMazeDataUpdated (MazeModel data)
		{
			_renderer.enabled = true;
		}

		public void Next (float moveTime)
		{
			if (moveTime > 0) {
				transform.DOMove (transform.position + new Vector3 (
					NodeModel.DIRECTIONS [_directionIdx, 0] * MazeController.NODE_SIZE, 
					NodeModel.DIRECTIONS [_directionIdx, 1] * MazeController.NODE_SIZE, 
				0
				), moveTime).OnComplete (OnReadyToProceed).SetEase (Ease.Linear);
			
				_cellPosition.x += NodeModel.DIRECTIONS [_directionIdx, 0];
				_cellPosition.y += NodeModel.DIRECTIONS [_directionIdx, 1];
			}
		}
		
		void OnReadyToProceed ()
		{
			DOTween.CompleteAll ();
			transform.eulerAngles = new Vector3 (0, 0, -90 * _directionIdx);
			NotificationManager.PLAYER_READY_TO_PROCEED.Dispatch (_cellPosition, _directionIdx);
		}

		void Proceed (NodeModel node, float moveTime)
		{
			Next (moveTime);
		}

		void OnExit(){
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
				SetDirection (NodeModel.DIRECTION_UP_IDX);
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				SetDirection (NodeModel.DIRECTION_RIGHT_IDX);
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				SetDirection (NodeModel.DIRECTION_DOWN_IDX);
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				SetDirection (NodeModel.DIRECTION_LEFT_IDX);
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
								SetDirection (NodeModel.DIRECTION_RIGHT_IDX);
							else
								SetDirection (NodeModel.DIRECTION_LEFT_IDX);
						} else {
							if (delta.y > 0) 
								SetDirection (NodeModel.DIRECTION_UP_IDX);
							else
								SetDirection (NodeModel.DIRECTION_DOWN_IDX);
						}
					}
				}
			}
		}
		
		public void SetDirection (int value)
		{
			_directionIdx = value;
					
			if (!DOTween.IsTweening (transform)) 
				OnReadyToProceed ();

			NotificationManager.PLAYER_DIRECTION_UPDATED.Dispatch (_directionIdx);
		}

		public void OnDestroy(){
			NotificationManager.MAZE_DATA_UPDATED.Remove (onMazeDataUpdated);
			NotificationManager.PROCEED.Remove (Proceed);
			NotificationManager.EXIT_REACHED.Remove (OnExit);
			NotificationManager.PLAYER_STUCK.Remove (OnStuck);
		}
	}
}
