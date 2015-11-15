using UnityEngine;
using System.Collections;
using DG.Tweening;
using Models;
using Models.Data;

public delegate void PlayerStepComplete ();

namespace Controllers {
	public class PlayerController : MonoBehaviour
	{
		public IntPoint cellPosition { get { return _cellPosition; } }

		private IntPoint _cellPosition;
		private int _directionIdx;
		private bool _didJustMove;
		private float _moveTime;
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
		}
		
		void UpdateMazeData (MazeModel data)
		{
			_renderer.enabled = true;
			_moveTime = data.config.moveTime;
		}

		public void Next (float moveTime, int rotateBy)
		{
			if (moveTime > 0) {
				_didJustMove = true;
				transform.DOMove (transform.position + new Vector3 (
					NodeModel.DIRECTIONS [_directionIdx, 0] * MazeController.NODE_SIZE, 
					NodeModel.DIRECTIONS [_directionIdx, 1] * MazeController.NODE_SIZE, 
				0
				), moveTime).OnComplete (OnStepCompleted).SetEase (Ease.Linear);
			
				_cellPosition.x += NodeModel.DIRECTIONS [_directionIdx, 0];
				_cellPosition.y += NodeModel.DIRECTIONS [_directionIdx, 1];

			} else {
				_didJustMove = false;
				if (rotateBy != 0) {
					transform.DORotate (transform.rotation.eulerAngles + new Vector3 (
						0, 0, rotateBy * -90), 0.4f).OnComplete (OnStepCompleted);
				}
				
				_directionIdx += rotateBy;
				if (_directionIdx >= NodeModel.DIRECTIONS.GetLength (0))
					_directionIdx = 0;
				
				if (_directionIdx < 0)
					_directionIdx = NodeModel.DIRECTIONS.GetLength (0) - 1;
					
			}
		}
		
		void OnStepCompleted ()
		{
			DOTween.CompleteAll ();
			transform.eulerAngles = new Vector3 (0, 0, -90 * _directionIdx);
			SendMessageUpwards ("OnStepComplete", _cellPosition);
		}

		void onNodeReached (NodeModel node)
		{
			if (node.HasFlag (NodeModel.SPECIALS_EXIT)) {
				_renderer.enabled = false;
				return;
			}
			
			float moveTime = _moveTime;
			if (node.HasFlag (NodeModel.SPECIALS_SPEEDUP_UP)) {
				if (_directionIdx == NodeModel.DIRECTION_UP_IDX)
					moveTime /= 2;
				
				if (_directionIdx == NodeModel.DIRECTION_DOWN_IDX)
					moveTime *= 2;
			}
			
			if (node.HasFlag (NodeModel.SPECIALS_SPEEDUP_RIGHT)) {
				if (_directionIdx == NodeModel.DIRECTION_RIGHT_IDX)
					moveTime /= 2;
				
				if (_directionIdx == NodeModel.DIRECTION_LEFT_IDX)
					moveTime *= 2;
			}
			
			if (node.HasFlag (NodeModel.SPECIALS_SPEEDUP_DOWN)) {
				if (_directionIdx == NodeModel.DIRECTION_DOWN_IDX)
					moveTime /= 2;
				
				if (_directionIdx == NodeModel.DIRECTION_UP_IDX)
					moveTime *= 2;
			}
			
			if (node.HasFlag (NodeModel.SPECIALS_SPEEDUP_LEFT)) {
				if (_directionIdx == NodeModel.DIRECTION_LEFT_IDX)
					moveTime /= 2;
				
				if (_directionIdx == NodeModel.DIRECTION_RIGHT_IDX)
					moveTime *= 2;
			}
			
			int rotateBy = 0;
			
			if (node.HasFlag (NodeModel.SPECIALS_ROTATOR_CW)) {
				rotateBy = 1;
			}
			
			if (node.HasFlag (NodeModel.SPECIALS_ROTATOR_CCW)) {
				rotateBy = -1;
			}
			
			if (!node.HasWall (_directionIdx) && (!_didJustMove || rotateBy == 0)) {
				rotateBy = 0;
				
				node.score = 0;
				Next (moveTime, rotateBy);
			} else {
				
				Next (-1, rotateBy);
				
				if (rotateBy == 0) {
					_audio.Play ();
					SendMessageUpwards ("onStuck");
				}
			}
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
				OnStepCompleted ();
		}
	}
}
