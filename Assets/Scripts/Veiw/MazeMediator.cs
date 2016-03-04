using UnityEngine;
using System.Collections;
using Model;
using Model.Data;
using Utils;
using DG.Tweening;
using System.Collections.Generic;
using Notifications;

namespace View
{
	public class MazeMediator : MonoBehaviour
	{
		public static float TRANSITION_TIME = 0.5f;

		//references for cleanup
		private List<GameObject> _nodeInstances = new List<GameObject> ();

		//flag for redrawing
		private bool _dirty;
		private int _prevMaxX = 0;
		private int _prevMaxY = 0;

		void Start ()
		{
			MazePaceNotifications.MAZE_RECREATED.Add (UpdateMazeData);
			MazePaceNotifications.NODE_REACHED.Add (OnNodeReached);
			MazePaceNotifications.TOGGLE_WALLS_VISIBILITY.Add (OnToggleWallsVisibility);
		}

		// Update is called once per frame
		void Update ()
		{
			if (_dirty) {
				Redraw ();
				_dirty = false;
			}
		}

		private void UpdateMazeData (MazeModel mazeData)
		{
			_dirty = true;
		}

		private void OnToggleWallsVisibility (bool value)
		{
			foreach (GameObject node in _nodeInstances)
				node.GetComponent<NodeMediator> ().ShowWall (value);
		}

		private void OnNodeReached (NodeVO node, float moveSpeed)
		{
			int index = node.pos.x * LevelModel.Instance ().width + node.pos.y;
			if (index < _nodeInstances.Count)
				_nodeInstances [index].GetComponent<NodeMediator> ().onReached ();
		}

		private void Redraw ()
		{	
			var levelModel = LevelModel.Instance ();
			var mazeModel = MazeModel.Instance ();

			transform.parent.DOMove (new Vector2 (
				-(levelModel.width - 1) * LevelModel.NODE_SIZE / 2, 
				-(levelModel.height - 1) * LevelModel.NODE_SIZE / 2
			), TRANSITION_TIME);

			foreach (Object nodeInstance in _nodeInstances)
				Destroy (nodeInstance);

			_nodeInstances.Clear ();

			//set of base colors
			ColorComponentVO[] colorComponents = ColorComponentVO.GetArray ();

			for (int cellX = 0; cellX < levelModel.width; cellX++) {
				for (int cellY = 0; cellY < levelModel.height; cellY++) {

					NodeVO node = mazeModel.GetNode (cellX, cellY);

					float[] tileRelativePos = new float[2] {
						(float)cellX / levelModel.width,
						(float)cellY / levelModel.height
					};

					float tint = 0.6f + 0.4f * (float)(node.score - levelModel.minScore) / (levelModel.maxScore - levelModel.minScore);

					//create a tile				
					GameObject nodeInstance = (GameObject)Instantiate (PrefabLib.NODE);
					nodeInstance.transform.parent = transform;
					nodeInstance.GetComponent<NodeMediator> ().Redraw (node, ColorComponentVO.GetColorAt (tileRelativePos, colorComponents, tint));
					_nodeInstances.Add (nodeInstance);

					float zOrder = 1 - (float)(cellY + cellX) / (levelModel.width + levelModel.height);	

					if (cellX <= _prevMaxX && cellY <= _prevMaxY)
						nodeInstance.transform.localPosition = new Vector3 (cellX * LevelModel.NODE_SIZE, cellY * LevelModel.NODE_SIZE, zOrder);
					else {
						nodeInstance.transform.localPosition = new Vector3 (Mathf.Min (_prevMaxX, cellX) * LevelModel.NODE_SIZE, Mathf.Min (_prevMaxY, cellY) * LevelModel.NODE_SIZE, zOrder);
						nodeInstance.transform.DOLocalMove (new Vector3 (cellX * LevelModel.NODE_SIZE, cellY * LevelModel.NODE_SIZE, zOrder), TRANSITION_TIME);
					}
				}
			}

			OnToggleWallsVisibility (true);
			_prevMaxX = levelModel.width - 1;
			_prevMaxY = levelModel.height - 1;
		}

		public void OnDestroy ()
		{
			MazePaceNotifications.MAZE_RECREATED.Remove (UpdateMazeData);
			MazePaceNotifications.NODE_REACHED.Remove (OnNodeReached);
			MazePaceNotifications.TOGGLE_WALLS_VISIBILITY.Remove (OnToggleWallsVisibility);
		}
	}
}
