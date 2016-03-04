using System;
using Model.Data;
using Notifications;
using UnityEngine;

namespace Controller
{
	public class ProcessInput
	{
		Vector2 _touchStartPoint;

		public ProcessInput ()
		{
		}

		// Update is called once per frame
		void Execute ()
		{
			//if gamestate is active
//			if (!_renderer.isVisible)//todo make it not binded to renderer
//				return;
			//keyboard input

			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				MazePaceNotifications.SET_PLAYER_DIRECTION.Dispatch (NodeVO.DIRECTION_UP_IDX);
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				MazePaceNotifications.SET_PLAYER_DIRECTION.Dispatch (NodeVO.DIRECTION_RIGHT_IDX);
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				MazePaceNotifications.SET_PLAYER_DIRECTION.Dispatch (NodeVO.DIRECTION_DOWN_IDX);
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				MazePaceNotifications.SET_PLAYER_DIRECTION.Dispatch (NodeVO.DIRECTION_LEFT_IDX);
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
								MazePaceNotifications.SET_PLAYER_DIRECTION.Dispatch (NodeVO.DIRECTION_RIGHT_IDX);
							else
								MazePaceNotifications.SET_PLAYER_DIRECTION.Dispatch (NodeVO.DIRECTION_LEFT_IDX);
						} else {
							if (delta.y > 0)
								MazePaceNotifications.SET_PLAYER_DIRECTION.Dispatch (NodeVO.DIRECTION_UP_IDX);
							else
								MazePaceNotifications.SET_PLAYER_DIRECTION.Dispatch (NodeVO.DIRECTION_DOWN_IDX);
						}
					}
				}
			}
		}
	}
}

