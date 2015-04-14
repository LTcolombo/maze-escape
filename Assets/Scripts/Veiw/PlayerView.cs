using UnityEngine;
using System.Collections;
using DG.Tweening;
using AssemblyCSharp;

public delegate void PlayerStepComplete ();

public class PlayerView : MonoBehaviour
{
	
		public static int STATE_NONE = 0;
		public static int STATE_LEFT = -1;
		public static int STATE_RIGHT = 1;
		public int state = STATE_NONE;
		public int directionIdx = 0;
		public int cellX = 4;
		public int cellY = 4;
	
		public event PlayerStepComplete onStepComplete;

		// Use this for initialization
		void Start ()
		{
				Invoke ("OnStepCompleted", 1);
		}
	
		public void Next ()
		{
				transform.DOMove (
				transform.localPosition + new Vector3 (
				MazeData.DIRECTIONS [directionIdx, 0] * 0.63f, 
				MazeData.DIRECTIONS [directionIdx, 1] * 0.63f, 
				0
				), 0.5f).OnComplete (OnStepCompleted);
				
				cellX += MazeData.DIRECTIONS [directionIdx, 0];
				cellY += MazeData.DIRECTIONS [directionIdx, 1];
		
		}
	
		void OnStepCompleted ()
		{
				ApplyDirection ();
				if (onStepComplete != null)
						onStepComplete ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
						OnLeft ();
				} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
						OnRight ();
				}
		}
	
		public void OnLeft ()
		{
				if (state > STATE_LEFT)
						state--;
		
				if (!DOTween.IsTweening (transform)) 
						OnStepCompleted ();
		}
	
		public void OnRight ()
		{
				if (state < STATE_RIGHT)
						state++;
		
				if (!DOTween.IsTweening (transform)) 
						OnStepCompleted ();
		}
	
		void ApplyDirection ()
		{
				
				if (state != 0) {
						directionIdx += state;
						if (directionIdx >= MazeData.DIRECTIONS.GetLength (0))
								directionIdx = 0;
			
						if (directionIdx < 0)
								directionIdx = MazeData.DIRECTIONS.GetLength (0) - 1;
						
						transform.eulerAngles = new Vector3 (0, 0, -90 * directionIdx);
			
						state = 0;
				}
		}
}

