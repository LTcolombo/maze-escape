using UnityEngine;
using System.Collections;
using Model;
using Model.Data;
using UnityEngine.UI;
using DG.Tweening;
using Notifications;

namespace View
{
	public class TutorialMediator : MonoBehaviour
	{
		Transform _handTransform;
		Renderer _handRenderer;

		int _correctDirection;
		int _playerDirection;
		
		// Use this for initialization
		void Start ()
		{
			GameObject handObject = GameObject.Find ("Hand");
			_handTransform = handObject.GetComponent<Transform> ();
			_handRenderer = handObject.GetComponent<Renderer> ();

			_correctDirection = NodeVO.DIRECTION_INVALID_IDX;
			MazePaceNotifications.MAZE_RECREATED.Add (OnMazeDataUpdated);
			MazePaceNotifications.PROCEED_FROM_NODE.Add (OnProceedFromNode);
			MazePaceNotifications.DIRECTION_UPDATED.Add (OnPlayerDirectionUpdated);
		}

		public void OnMazeDataUpdated ()
		{
			_handRenderer.enabled = true;
			UpdateCorrectDirection ();
		}

		void OnProceedFromNode (NodeVO node)
		{
			UpdateCorrectDirection ();
		}

		void UpdateCorrectDirection ()
		{
			IntPointVO cell = PlayerModel.Instance ().cellPosition;
			var currentNode = MazeModel.Instance ().GetNode (cell.x, cell.y);
			int newDirection = currentNode.directionToExit;
			if (newDirection == NodeVO.DIRECTION_INVALID_IDX)
				return;

			if (currentNode.HasFlag (NodeVO.SPECIALS_ROTATOR_CW))
				newDirection--;

			if (currentNode.HasFlag (NodeVO.SPECIALS_ROTATOR_CCW))
				newDirection++;

			if (newDirection > 3)
				newDirection = 0;
			
			if (newDirection < 0)
				newDirection = 3;
			
			if (_correctDirection == newDirection) {
				return;
			}
	
			_correctDirection = newDirection;
			_handRenderer.enabled = _playerDirection != _correctDirection;
			
			DOTween.Kill (_handTransform);
			
			Vector2 start = new Vector2 (0, -Camera.main.orthographicSize * 0.5f);
			Vector2 stop = new Vector2 (0, -Camera.main.orthographicSize * 0.5f);
			
			switch (_correctDirection) {
			case (NodeVO.DIRECTION_UP_IDX):
				start.y -= 50.0f;
				stop.y += 50.0f;
				break;
				
			case (NodeVO.DIRECTION_RIGHT_IDX):
				start.x -= 50.0f;
				stop.x += 50.0f;
				break;
				
			case (NodeVO.DIRECTION_DOWN_IDX):
				start.y += 50.0f;
				stop.y -= 50.0f;
				break;
				
			case (NodeVO.DIRECTION_LEFT_IDX):
				start.x += 50.0f;
				stop.x -= 50.0f;
				break;
			}
			
			_handTransform.position = start;
			_handTransform.DOMove (stop, 0.5f).SetLoops (-1).SetEase (Ease.InOutCubic);
		}

		void OnPlayerDirectionUpdated (int value)
		{
			if (_playerDirection == value)
				return;
		
			_playerDirection = value;
			_handRenderer.enabled = _playerDirection != _correctDirection;
		}

		void OnExitReached ()
		{
			_handRenderer.enabled = false;
		}

		void OnDestroy ()
		{
			MazePaceNotifications.MAZE_RECREATED.Remove (OnMazeDataUpdated);
			MazePaceNotifications.PROCEED_FROM_NODE.Remove (OnProceedFromNode);
			MazePaceNotifications.DIRECTION_UPDATED.Remove (OnPlayerDirectionUpdated);
		}
	}
}

