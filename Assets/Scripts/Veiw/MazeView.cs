using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using DG.Tweening;
using System.Collections.Generic;

public class MazeView : MonoBehaviour
{
	public static int NODE_SIZE = 128;
	public static float TRANSITION_TIME = 0.5f;
			
	//current maze data
	private MazeData _mazeData;
	
	//references for cleanup
	private List<GameObject> _nodeInstances = new List<GameObject> ();
	
	//flag for redrawing
	private bool _dirty;
	private int _prevMaxX = 0;
	private int _prevMaxY = 0;
		
	// Use this for initialization
	void Start ()
	{
	}		
	
	// Update is called once per frame
	void Update ()
	{
		if (_dirty) {
			Redraw ();
			ShowWalls (true);
			_dirty = false;
			
			_prevMaxX = _mazeData.config.width - 1;
			_prevMaxY = _mazeData.config.height - 1;
		}
	}
	
	public void UpdateMazeData (MazeData mazeData)
	{
		_mazeData = mazeData;
		_dirty = true;
	}
	
	public void ShowWalls (bool value)
	{
		foreach (GameObject node in _nodeInstances)
			node.GetComponent<NodeView> ().ShowWall (value);
	}
	
	public void DesaturateTileAt (int x, int y)
	{
		int index = x * _mazeData.config.width + y;
		if (index < _nodeInstances.Count) 
			_nodeInstances [index].GetComponent<NodeView>().Desaturate();
	}
		
	private void Redraw ()
	{
		foreach (Object nodeInstance in _nodeInstances)
			Destroy (nodeInstance);
		
		_nodeInstances.Clear ();
				
		//set of base colors
		ColorComponent[] colorComponents = ColorComponent.GetArray ();
				
		for (int cellX = 0; cellX < _mazeData.config.width; cellX++) {
			for (int cellY = 0; cellY < _mazeData.config.height; cellY++) {
			
				NodeData node = _mazeData.GetNode (cellX, cellY);
				
				float[] tileRelativePos = new float[2] {
					(float)cellX / _mazeData.config.width,
					(float)cellY / _mazeData.config.height
					};
				
				float tint = 0.6f + 0.4f * (float)(node.score-_mazeData.config.minScore) / (_mazeData.config.maxScore-_mazeData.config.minScore);
								
				//create a tile				
				GameObject nodeInstance = (GameObject)Instantiate (Prefabs.NODE);
				nodeInstance.transform.parent = transform;
				nodeInstance.GetComponent<NodeView> ().Redraw (node, ColorComponent.GetColorAt (tileRelativePos, colorComponents, tint));
				_nodeInstances.Add (nodeInstance);

				float zOrder = 1 - (float)(cellY + cellX) / (_mazeData.config.width + _mazeData.config.height);	

				if (cellX <= _prevMaxX && cellY <= _prevMaxY)
					nodeInstance.transform.localPosition = new Vector3 (cellX * NODE_SIZE, cellY * NODE_SIZE, zOrder);
				else {
					nodeInstance.transform.localPosition = new Vector3 (Mathf.Min (_prevMaxX, cellX) * NODE_SIZE, Mathf.Min (_prevMaxY, cellY) * NODE_SIZE, zOrder);
					nodeInstance.transform.DOLocalMove (new Vector3 (cellX * NODE_SIZE, cellY * NODE_SIZE, zOrder), TRANSITION_TIME);
				}
			}
		}
	}
}

