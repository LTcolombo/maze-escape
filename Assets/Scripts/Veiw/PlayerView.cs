using UnityEngine;
using System.Collections;
using DG.Tweening;
using AssemblyCSharp;

public delegate void PlayerStepComplete ();

public class PlayerView : MonoBehaviour
{
	public int cellX { get { return _cellX; } }

	public int cellY { get { return _cellY; } }

	public int directionIdx { get { return _directionIdx; } }

	public bool didJustMove { get { return _didJustMove; } }
	
	private int _cellX = 2;
	private int _cellY;
	private int _directionIdx;
	private bool _didJustMove;
	
	public event PlayerStepComplete onStepComplete;

	private Vector2 _touchStartPoint;

	// Use this for initialization
	void Start ()
	{
		_directionIdx = NodeData.DIRECTION_UP_IDX;
		transform.eulerAngles = new Vector3 (0, 0, -90 * _directionIdx);
		transform.localPosition = new Vector3(MazeView.NODE_SIZE *_cellX, 
		                                      MazeView.NODE_SIZE *_cellY, 
		                                      0);
	}

	public void InvokeAutostartIn (int value)
	{
		Invoke ("AutoStart", value);
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
		
			_cellX += NodeData.DIRECTIONS [_directionIdx, 0];
			_cellY += NodeData.DIRECTIONS [_directionIdx, 1];

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
		
	void AutoStart ()
	{
		if (!DOTween.IsTweening (transform)) 
			OnStepCompleted ();
	}
	
	void OnStepCompleted ()
	{
		DOTween.CompleteAll ();
		transform.eulerAngles = new Vector3 (0, 0, -90 * _directionIdx);
		if (onStepComplete != null)
			onStepComplete ();
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

