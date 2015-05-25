using UnityEngine;
using System.Collections;
using DG.Tweening;
using AssemblyCSharp;

public delegate void PlayerStepComplete ();

public class PlayerView : MonoBehaviour
{
	public static float MOVE_TIME = 0.4f;
	public int directionIdx;
	public int cellX;
	public int cellY;
	
	public event PlayerStepComplete onStepComplete;

	Vector2 _startPoint;

	// Use this for initialization
	void Start ()
	{
		directionIdx = NodeData.DIRECTION_UP_IDX;
		transform.eulerAngles = new Vector3 (0, 0, -90 * directionIdx);
	}

	public void InvokeAutostartIn (int value)
	{
		Invoke ("AutoStart", value);
	}
	
	public void Next ()
	{
		transform.DOMove (transform.position + new Vector3 (
			NodeData.DIRECTIONS [directionIdx, 0] * MazeView.NODE_SIZE, 
			NodeData.DIRECTIONS [directionIdx, 1] * MazeView.NODE_SIZE, 
			0
		), MOVE_TIME).OnComplete (OnStepCompleted).SetEase (Ease.Linear);
				
		cellX += NodeData.DIRECTIONS [directionIdx, 0];
		cellY += NodeData.DIRECTIONS [directionIdx, 1];
	}
		
	void AutoStart ()
	{
		if (!DOTween.IsTweening (transform)) 
			OnStepCompleted ();
	}
	
	void OnStepCompleted ()
	{
		transform.eulerAngles = new Vector3 (0, 0, -90 * directionIdx);
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
					_startPoint = touch.position;
				} else if (touch.phase == TouchPhase.Ended) {
								
					Vector2 delta = touch.position - _startPoint;
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
		directionIdx = value;
				
		if (!DOTween.IsTweening (transform)) 
			OnStepCompleted ();
	}
}

