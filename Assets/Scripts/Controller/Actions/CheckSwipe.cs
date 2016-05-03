using Notifications;
using UnityEngine;
using Model.Data;

namespace Controller
{
	public class CheckSwipe:Action
	{
		Vector2 _touchStartPoint;

		// Update is called once per frame
		override public PrefromResult Perform (float delta)
		{
			//keyboard input
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				MazePaceNotifications.DIRECTION_UPDATED.Dispatch (NodeVO.DIRECTION_UP_IDX);
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				MazePaceNotifications.DIRECTION_UPDATED.Dispatch (NodeVO.DIRECTION_RIGHT_IDX);
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				MazePaceNotifications.DIRECTION_UPDATED.Dispatch (NodeVO.DIRECTION_DOWN_IDX);
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				MazePaceNotifications.DIRECTION_UPDATED.Dispatch (NodeVO.DIRECTION_LEFT_IDX);
			} 

			if (Input.touchCount > 0) {
				for (int i = 0; i < Input.touchCount; i++) {
					Touch touch = Input.GetTouch (i);

					if (touch.phase == TouchPhase.Began) {
						_touchStartPoint = touch.position;
					} else if (touch.phase == TouchPhase.Ended) {

						Vector2 deltaPosition = touch.position - _touchStartPoint;
						if (deltaPosition.magnitude == 0)
							continue;

						if (Mathf.Abs (deltaPosition.x) > Mathf.Abs (deltaPosition.y)) {
							if (deltaPosition.x > 0)
								MazePaceNotifications.DIRECTION_UPDATED.Dispatch (NodeVO.DIRECTION_RIGHT_IDX);
							else
								MazePaceNotifications.DIRECTION_UPDATED.Dispatch (NodeVO.DIRECTION_LEFT_IDX);
						} else {
							if (deltaPosition.y > 0)
								MazePaceNotifications.DIRECTION_UPDATED.Dispatch (NodeVO.DIRECTION_UP_IDX);
							else
								MazePaceNotifications.DIRECTION_UPDATED.Dispatch (NodeVO.DIRECTION_DOWN_IDX);
						}
					}
				}
			}

			return PrefromResult.PROCEED;
		}
	}
}

