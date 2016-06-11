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

		private void UpdateMazeData ()
		{
			_dirty = true;
		}

		private void OnToggleWallsVisibility (bool value)
		{
			foreach (GameObject node in _nodeInstances)
				node.GetComponent<NodeMediator> ().ShowWall (value);
		}

		private void OnNodeReached ()
		{
			var cellPosition = PlayerModel.Instance ().cellPosition;
			int index = cellPosition.x * MazeModel.Instance ().size + cellPosition.y;
			if (index < _nodeInstances.Count)
				_nodeInstances [index].GetComponent<NodeMediator> ().onReached ();
		}

		private void Redraw ()
		{	
			var difficultyModel = DifficultyModel.Instance ();
			var mazeModel = MazeModel.Instance ();

			transform.parent.localPosition = new Vector2 (
				-(mazeModel.size - 1) * DifficultyModel.NODE_SIZE / 2, 
				-(mazeModel.size - 1) * DifficultyModel.NODE_SIZE / 2
			);

			//set of base colors
			ColorComponentVO[] colorComponents = ColorComponentVO.GetArray ();

			int index = 0;
			for (int cellX = 0; cellX < mazeModel.size; cellX++) {
				for (int cellY = 0; cellY < mazeModel.size; cellY++) {
					NodeVO node = mazeModel.GetNode (cellX, cellY);

					float[] tileRelativePos = new float[2] {
						(float)cellX / mazeModel.size,
						(float)cellY / mazeModel.size
					};

					float tint = 0.6f + 0.4f * (float)(node.score - difficultyModel.minScore) / (difficultyModel.maxScore - difficultyModel.minScore);

					GameObject nodeInstance;
					if (_nodeInstances.Count > index) {
						nodeInstance = _nodeInstances [index];
					}
					else {
						//create a tile				
						nodeInstance = (GameObject)Instantiate (PrefabLib.NODE);
						nodeInstance.transform.parent = transform;
						_nodeInstances.Add (nodeInstance);
					}

					float zOrder = 10 - (float)(cellY + cellX) / (mazeModel.size + mazeModel.size);
					nodeInstance.GetComponent<NodeMediator> ().Redraw (node, ColorComponentVO.GetColorAt (tileRelativePos, colorComponents, tint));
					nodeInstance.transform.localPosition = new Vector3 (cellX * DifficultyModel.NODE_SIZE, cellY * DifficultyModel.NODE_SIZE, zOrder);

					index++;
				}
			}
		}

		public void OnDestroy ()
		{
			MazePaceNotifications.MAZE_RECREATED.Remove (UpdateMazeData);
			MazePaceNotifications.NODE_REACHED.Remove (OnNodeReached);
			MazePaceNotifications.TOGGLE_WALLS_VISIBILITY.Remove (OnToggleWallsVisibility);
		}
	}
}
