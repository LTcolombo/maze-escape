using UnityEngine;
using System.Collections;
using Models;
using UnityEngine.UI;
using DG.Tweening;
using Notifications;

namespace Views
{
	public class TutorialMediator : MonoBehaviour
	{
		GameObject _handObject;
		Transform _handTransform;
		SpriteRenderer _handRenderer;
		int _lastState;
		int _correctDirection;
		int _playerDirection;
		string[] directions = { "up", "right", "down", "left" };
		int[,] _directionsToExit;
		
		// Use this for initialization
		void Start ()
		{
			_handObject = GameObject.Find ("Hand");
			_handTransform = _handObject.GetComponent<Transform> ();
			_handRenderer = _handObject.GetComponent<SpriteRenderer> ();
			
			_lastState = GameStateModel.STATE_INVALID;
			_correctDirection = NodeModel.DIRECTION_INVALID_IDX;
			NotificationManager.GAME_STATE_UPDATED.Add (OnGameStateUpdated);
			NotificationManager.NODE_PASSED.Add (OnNodePassed);
			NotificationManager.MAZE_DATA_UPDATED.Add (OnMazeDataUpdated);
			NotificationManager.PLAYER_DIRECTION_UPDATED.Add (OnPlayerDirectionUpdated);
		}

		public void OnMazeDataUpdated (MazeModel mazeData)
		{
			enabled = mazeData.config.isTutorial;
			OnNodePassed (mazeData.startingNode);
			
			_directionsToExit = new int[mazeData.config.width, mazeData.config.height];
			for (int x = 0; x < mazeData.config.width; x++) {
				for (int y = 0; y < mazeData.config.width; y++) {
					_directionsToExit [x, y] = NodeModel.DIRECTION_INVALID_IDX;
				}
			}

			findDirectionsTo (mazeData, mazeData.deadEnds [0]);
			for (int y = 0; y < _directionsToExit.GetLength (0); y++) {
				string str = "";
				for (int x = 0; x < _directionsToExit.GetLength (1); x++)
					str += _directionsToExit [x, y] + ",";
					
				Debug.Log (str);
			}
		}

		void findDirectionsTo (MazeModel maze, NodeModel node)
		{
			for (int directionIdx = NodeModel.DIRECTION_UP_IDX; directionIdx <= NodeModel.DIRECTION_LEFT_IDX; directionIdx++) {
				int x = node.pos.x;
				int y = node.pos.y;
				int directionToExit = NodeModel.DIRECTION_INVALID_IDX;
				
				if (node.HasWall (directionIdx)) {
					continue;
				}
				
				switch (directionIdx) {
				case (NodeModel.DIRECTION_UP_IDX):
					directionToExit = NodeModel.DIRECTION_DOWN_IDX;
					y++;
					break;
					
				case (NodeModel.DIRECTION_RIGHT_IDX):
					directionToExit = NodeModel.DIRECTION_LEFT_IDX;
					x++;
					break;
					
				case (NodeModel.DIRECTION_DOWN_IDX):
					directionToExit = NodeModel.DIRECTION_UP_IDX;
					y--;
					break;
					
				case (NodeModel.DIRECTION_LEFT_IDX):
					directionToExit = NodeModel.DIRECTION_RIGHT_IDX;
					x--;
					break;
				}
				if (maze.IsInBounds (x, y) && _directionsToExit [x, y] == NodeModel.DIRECTION_INVALID_IDX) {
					_directionsToExit [x, y] = directionToExit;
					findDirectionsTo (maze, maze.GetNode (x, y));
				}
			}
		}

		void OnNodePassed (NodeModel node)
		{
			if (_lastState != GameStateModel.STATE_ACTIVATED) {
				if (node.nextNode != null) {
					SetCorrectDirection (node.GetDirectionTowards (node.nextNode));
				}
			} else {
				//check if there are two nodes ahead to define needed direction
				if (node.nextNode != null && node.nextNode.nextNode != null) {
					SetCorrectDirection (node.nextNode.GetDirectionTowards (node.nextNode.nextNode));
				}
			}
		}

		void OnGameStateUpdated (GameStateModel state)
		{
			if (_lastState == state.state)
				return;
			
			_lastState = state.state;
			switch (state.state) {
			case (GameStateModel.STATE_STUCK):
				_handRenderer.color = new Color (0.8f, 0.2f, 0.2f);
				break;
				
			default:
				_handRenderer.color = new Color (1f, 1f, 1f);
				break;
			}
		}

		void SetCorrectDirection (int value)
		{
			Debug.Log ("Correct Direction: " + directions [value]);
			if (_correctDirection == value) {
				return;
			}
	
			_correctDirection = value;
			
			var active = true;//_playerDirection != _correctDirection;
			_handObject.SetActive (active);
			
			if (!active) {
				return;
			}
			
			DOTween.Kill (_handTransform);
			
			Vector2 start = new Vector2 (0, -Camera.main.orthographicSize * 0.5f);
			Vector2 stop = new Vector2 (0, -Camera.main.orthographicSize * 0.5f);
			
			switch (_correctDirection) {
			case (NodeModel.DIRECTION_UP_IDX):
				start.y -= 50.0f;
				stop.y += 50.0f;
				break;
				
			case (NodeModel.DIRECTION_RIGHT_IDX):
				start.x -= 50.0f;
				stop.x += 50.0f;
				break;
				
			case (NodeModel.DIRECTION_DOWN_IDX):
				start.y += 50.0f;
				stop.y -= 50.0f;
				break;
				
			case (NodeModel.DIRECTION_LEFT_IDX):
				start.x += 50.0f;
				stop.x -= 50.0f;
				break;
			}
			
			_handTransform.position = start;
			_handTransform.DOMove (stop, 0.8f).SetLoops (-1).SetEase (Ease.InOutCubic);
				
		}

		void OnPlayerDirectionUpdated (int value)
		{
			Debug.Log ("Player Direction: " + directions [value]);
			if (_playerDirection == value)
				return;
		
			_playerDirection = value;
			//_handObject.SetActive (_playerDirection != _correctDirection);
		}

		void OnDestroy ()
		{
			NotificationManager.GAME_STATE_UPDATED.Remove (OnGameStateUpdated);
			NotificationManager.NODE_PASSED.Remove (OnNodePassed);
			NotificationManager.MAZE_DATA_UPDATED.Remove (OnMazeDataUpdated);
			NotificationManager.PLAYER_DIRECTION_UPDATED.Remove (OnPlayerDirectionUpdated);
		}
	}
}

