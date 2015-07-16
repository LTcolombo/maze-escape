using UnityEngine;
using System.Collections;
using DG.Tweening;
using AssemblyCSharp;

public delegate void PlayerStepComplete ();

public class PlayerController : MonoBehaviour
{
	public IntPoint cellPosition { get { return _cellPosition; } }

	public int directionIdx { get { return _directionIdx; } }

	public bool didJustMove { get { return _didJustMove; } }
	
	private IntPoint _cellPosition;
	private int _directionIdx;
	private bool _didJustMove;

	private Vector2 _touchStartPoint;
	
	private AudioSource _audio;

	// Use this for initialization
	void Start ()
	{
		_cellPosition = new IntPoint (0, 0);

		_directionIdx = NodeData.DIRECTION_UP_IDX;
		transform.eulerAngles = new Vector3 (0, 0, -90 * _directionIdx);
		transform.localPosition = new Vector3(MazeView.NODE_SIZE * _cellPosition.x, 
		                                      MazeView.NODE_SIZE * _cellPosition.y, 
		                                      0);
		                                      
		_audio = GetComponent<AudioSource>();
	}

	public void Next (float moveTime, int rotateBy)
	{
		if (moveTime > 0) {
			_didJustMove = true;
			transform.DOMove (transform.position + new Vector3 (
				NodeData.DIRECTIONS [_directionIdx, 0] * MazeView.NODE_SIZE, 
				NodeData.DIRECTIONS [_directionIdx, 1] * MazeView.NODE_SIZE, 
			0
			), moveTime).OnComplete (OnStepCompleted).SetEase (Ease.Linear);
		
			_cellPosition.x += NodeData.DIRECTIONS [_directionIdx, 0];
			_cellPosition.y += NodeData.DIRECTIONS [_directionIdx, 1];

		} else {
			_didJustMove = false;
			if (rotateBy != 0) {
				transform.DORotate (transform.rotation.eulerAngles + new Vector3 (
					0, 0, rotateBy * -90), 0.4f).OnComplete (OnStepCompleted);
			}
			
			_directionIdx += rotateBy;
			if (_directionIdx >= NodeData.DIRECTIONS.GetLength (0))
				_directionIdx = 0;
			
			if (_directionIdx < 0)
				_directionIdx = NodeData.DIRECTIONS.GetLength (0) - 1;
				
		}
	}
	
	void OnStepCompleted ()
	{
		DOTween.CompleteAll ();
		transform.eulerAngles = new Vector3 (0, 0, -90 * _directionIdx);
		_audio.Play();
		SendMessageUpwards ("onStepComplete", _cellPosition);
	}

	void onNodeReached (NodeData node){
		if (node.HasFlag (NodeData.SPECIALS_EXIT)) {
			SendMessageUpwards("onExit", cellPosition);
			SetActive (false);
			return;
		}
		
		float moveTime = _mazeData.config.moveTime;
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_UP)) {
			if (directionIdx == NodeData.DIRECTION_UP_IDX)
				moveTime /= 2;
			
			if (directionIdx == NodeData.DIRECTION_DOWN_IDX)
				moveTime *= 2;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_RIGHT)) {
			if (directionIdx == NodeData.DIRECTION_RIGHT_IDX)
				moveTime /= 2;
			
			if (directionIdx == NodeData.DIRECTION_LEFT_IDX)
				moveTime *= 2;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_DOWN)) {
			if (directionIdx == NodeData.DIRECTION_DOWN_IDX)
				moveTime /= 2;
			
			if (directionIdx == NodeData.DIRECTION_UP_IDX)
				moveTime *= 2;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_LEFT)) {
			if (directionIdx == NodeData.DIRECTION_LEFT_IDX)
				moveTime /= 2;
			
			if (directionIdx == NodeData.DIRECTION_RIGHT_IDX)
				moveTime *= 2;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_HIDE_WALLS)) {
			BroadcastMessage ("ShowWalls", false);
		}
		
		if (node.HasFlag (NodeData.SPECIALS_SHOW_WALLS)) {
			BroadcastMessage ("ShowWalls", true);
		}
		
		int rotateBy = 0;
		
		if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CW)) {
			rotateBy = 1;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CCW)) {
			rotateBy = -1;
		}
		
		if (!node.HasWall (directionIdx) && (!didJustMove || rotateBy == 0)) {
			rotateBy = 0;
			
			node.score = 0;
			Next (moveTime, rotateBy);
		} else {
			
			Next (-1, rotateBy);
			
			if (rotateBy == 0) {
				SendMessageUpwards("onStuck");
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		//keyboard input
		
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			SetDirection (NodeData.DIRECTION_UP_IDX);
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			SetDirection (NodeData.DIRECTION_RIGHT_IDX);
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			SetDirection (NodeData.DIRECTION_DOWN_IDX);
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			SetDirection (NodeData.DIRECTION_LEFT_IDX);
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
							SetDirection (NodeData.DIRECTION_RIGHT_IDX);
						else
							SetDirection (NodeData.DIRECTION_LEFT_IDX);
					} else {
						if (delta.y > 0) 
							SetDirection (NodeData.DIRECTION_UP_IDX);
						else
							SetDirection (NodeData.DIRECTION_DOWN_IDX);
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

