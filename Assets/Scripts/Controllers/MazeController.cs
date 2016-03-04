using UnityEngine;
using System.Collections;
using Model;
using Model.Data;
using Utils;
using DG.Tweening;
using System.Collections.Generic;
using Notifications;

namespace Controllers {
	public class MazeController : MonoBehaviour
	{
		public static float TRANSITION_TIME = 0.5f;
				
		//current maze data
		private MazeModel _mazeData;
		
		//references for cleanup
		private List<GameObject> _nodeInstances = new List<GameObject> ();
		
		//flag for redrawing
		private bool _dirty;
		private int _prevMaxX = 0;
		private int _prevMaxY = 0;
				
		void Start(){
			MazePaceNotifications.MAZE_RECREATED.Add (UpdateMazeData);
			MazePaceNotifications.PROCEED.Add (Proceed);
		}

		// Update is called once per frame
		void Update ()
		{
			if (_dirty) {
				Redraw ();
				ShowWalls (true);
				_dirty = false;
				_prevMaxX = LevelModel.Instance().width - 1;
				_prevMaxY = LevelModel.Instance().height - 1;
			}
		}
		
		public void UpdateMazeData (MazeModel mazeData)
		{
			_mazeData = mazeData;
			_dirty = true;
		}
		
		public void ShowWalls (bool value)
		{
			foreach (GameObject node in _nodeInstances)
				node.GetComponent<NodeController> ().ShowWall (value);
		}
		
		void Proceed (NodeVO node, float moveSpeed)
		{
			int index = node.pos.x * LevelModel.Instance().width + node.pos.y;
			if (index < _nodeInstances.Count) 
				_nodeInstances [index].GetComponent<NodeController>().onReached();
		
			if (node.HasFlag (NodeVO.SPECIALS_HIDE_WALLS)) {
				ShowWalls(false);
			}
			
			if (node.HasFlag (NodeVO.SPECIALS_SHOW_WALLS)) {
				ShowWalls(true);
			}		
		}
			
		private void Redraw ()
		{	
			transform.parent.DOMove (new Vector2 (
				-(LevelModel.Instance().width - 1) * LevelModel.NODE_SIZE / 2, 
				-(LevelModel.Instance().height - 1) * LevelModel.NODE_SIZE / 2
				), TRANSITION_TIME);
		
			foreach (Object nodeInstance in _nodeInstances)
				Destroy (nodeInstance);
			
			_nodeInstances.Clear ();
					
			//set of base colors
			ColorComponentVO[] colorComponents = ColorComponentVO.GetArray ();
					
			for (int cellX = 0; cellX < LevelModel.Instance().width; cellX++) {
				for (int cellY = 0; cellY < LevelModel.Instance().height; cellY++) {
				
					NodeVO node = _mazeData.GetNode (cellX, cellY);
					
					float[] tileRelativePos = new float[2] {
						(float)cellX / LevelModel.Instance().width,
						(float)cellY / LevelModel.Instance().height
						};
					
					float tint = 0.6f + 0.4f * (float)(node.score-LevelModel.Instance().minScore) / (LevelModel.Instance().maxScore-LevelModel.Instance().minScore);
									
					//create a tile				
					GameObject nodeInstance = (GameObject)Instantiate (PrefabLib.NODE);
					nodeInstance.transform.parent = transform;
					nodeInstance.GetComponent<NodeController> ().Redraw (node, ColorComponentVO.GetColorAt (tileRelativePos, colorComponents, tint));
					_nodeInstances.Add (nodeInstance);

					float zOrder = 1 - (float)(cellY + cellX) / (LevelModel.Instance().width + LevelModel.Instance().height);	

					if (cellX <= _prevMaxX && cellY <= _prevMaxY)
						nodeInstance.transform.localPosition = new Vector3 (cellX * LevelModel.NODE_SIZE, cellY * LevelModel.NODE_SIZE, zOrder);
					else {
						nodeInstance.transform.localPosition = new Vector3 (Mathf.Min (_prevMaxX, cellX) * LevelModel.NODE_SIZE, Mathf.Min (_prevMaxY, cellY) * LevelModel.NODE_SIZE, zOrder);
						nodeInstance.transform.DOLocalMove (new Vector3 (cellX * LevelModel.NODE_SIZE, cellY * LevelModel.NODE_SIZE, zOrder), TRANSITION_TIME);
					}
				}
			}
		}

		public void OnDestroy(){
			MazePaceNotifications.MAZE_RECREATED.Remove (UpdateMazeData);
			MazePaceNotifications.PROCEED.Remove (Proceed);
		}
	}
}
