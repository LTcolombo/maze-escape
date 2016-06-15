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
		public static float TRANSITION_TIME = 0.3f;

		//references for cleanup
		private List<GameObject> _nodeInstances = new List<GameObject> ();

		//flag for redrawing
		private bool _dirty;

		void Start ()
		{
			MazePaceNotifications.MAZE_RECREATED.Add (UpdateMazeData);
			MazePaceNotifications.NODE_REACHED.Add (OnNodeReached);
			MazePaceNotifications.EXIT_REACHED.Add (OnExitReached);
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

		private void OnExitReached ()
		{
			var maze = MazeModel.Instance ();
			for (int i = 0; i < _nodeInstances.Count; i++) {
				int value = Random.Range (0, 3);
				GameObject node = _nodeInstances [i];
				var position = node.transform.position;
				if (Random.Range (0, 2) == 1)
					node.transform.DOLocalMoveX ((Random.Range (0, 2) == 1 ? value + maze.size : -(value + 1)) * DifficultyModel.NODE_SIZE, TRANSITION_TIME);
				else
					node.transform.DOLocalMoveY ((Random.Range (0, 2) == 1 ? value + maze.size : -(value + 1)) * DifficultyModel.NODE_SIZE, TRANSITION_TIME);
				
			}
		}

		private void Redraw ()
		{	
			var difficultyModel = DifficultyModel.Instance ();
			var maze = MazeModel.Instance ();

			transform.parent.localPosition = new Vector2 (
				-(maze.size - 1) * DifficultyModel.NODE_SIZE / 2, 
				-(maze.size - 1) * DifficultyModel.NODE_SIZE / 2
			);

			//set of base colors
			ColorComponentVO[] colorComponents = ColorComponentVO.GetArray ();

			int index = 0;
			for (int cellX = 0; cellX < maze.size; cellX++) {
				for (int cellY = 0; cellY < maze.size; cellY++) {
					NodeVO node = maze.GetNode (cellX, cellY);

					float[] tileRelativePos = new float[2] {
						(float)cellX / maze.size,
						(float)cellY / maze.size
					};

					float tint = 0.6f + 0.4f * (float)(node.score - difficultyModel.minScore) / (difficultyModel.maxScore - difficultyModel.minScore);
					float zOrder = 10 - (float)(cellY + cellX) / (maze.size + maze.size);

					GameObject nodeInstance;
					if (_nodeInstances.Count > index) {
						nodeInstance = _nodeInstances [index];
					} else {
						//create a tile				
						nodeInstance = (GameObject)Instantiate (PrefabLib.NODE);
						nodeInstance.transform.parent = transform;
						int value = Random.Range (0, 3);
						int randompos = (Random.Range (0, 2) == 1 ? value + maze.size : -(value + 1));
						if (Random.Range (0, 2) == 1)
							nodeInstance.transform.localPosition = new Vector3 (cellX * DifficultyModel.NODE_SIZE, randompos * DifficultyModel.NODE_SIZE, zOrder);
						else
							nodeInstance.transform.localPosition = new Vector3 (randompos * DifficultyModel.NODE_SIZE, cellY * DifficultyModel.NODE_SIZE, zOrder);
						_nodeInstances.Add (nodeInstance);
					}

					nodeInstance.transform.DOLocalMove (new Vector3 (
						cellX * DifficultyModel.NODE_SIZE, 
						cellY * DifficultyModel.NODE_SIZE, 
						zOrder
					), TRANSITION_TIME);

					nodeInstance.GetComponent<NodeMediator> ().Redraw (node, ColorComponentVO.GetColorAt (tileRelativePos, colorComponents, tint));

					index++;
				}
			}
		}

		public void OnDestroy ()
		{
			MazePaceNotifications.MAZE_RECREATED.Remove (UpdateMazeData);
			MazePaceNotifications.NODE_REACHED.Remove (OnNodeReached);
			MazePaceNotifications.EXIT_REACHED.Remove (OnExitReached);
			MazePaceNotifications.TOGGLE_WALLS_VISIBILITY.Remove (OnToggleWallsVisibility);
		}
	}
}
