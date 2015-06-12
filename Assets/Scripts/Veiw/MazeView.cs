using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using DG.Tweening;

public class MazeView : MonoBehaviour
{
	public static int NODE_SIZE = 128;
	public static float TRANSITION_TIME = 0.5f;
	
	//containers
	private GameObject _wallsContainer;
	private GameObject _genericContainer;
		
	//current maze data
	private MazeData _mazeData;
	
	//references for cleanup
	private ArrayList _tileInstances = new ArrayList ();
	private ArrayList _objectInstances = new ArrayList ();
	
	//flag for redrawing
	private bool _dirty;
	private int _prevMaxX = 0;
	private int _prevMaxY = 0;
		
	// Use this for initialization
	void Start ()
	{
		
		_wallsContainer = CreateContainer ("Walls");
		_genericContainer = CreateContainer ("Objects");
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
		_wallsContainer.transform.localPosition = new Vector3 (0, 0, value ? 0 : 2);
	}
	
	public void DesaturateTileAt (int x, int y)
	{
		int index = x * _mazeData.config.width + y;
		if (index < _tileInstances.Count) {
			SpriteRenderer renderer = ((GameObject)_tileInstances [index]).GetComponent<SpriteRenderer> ();
			//normalize color
			float sumColor = renderer.color.r + renderer.color.g + renderer.color.b; 
			renderer.color = new Color (sumColor / 3, sumColor / 3, sumColor / 3, 1f);
		}
	}
		
	private void Redraw ()
	{
		
		foreach (Object wallInstance in _objectInstances)
			Destroy (wallInstance);
						
		_objectInstances.Clear ();
				
		
		foreach (Object tileInstance in _tileInstances)
			Destroy (tileInstance);
		
		_tileInstances.Clear ();
		
		
		//set of base colors
		ColorComponent[] colorComponents = ColorComponent.GetArray ();
				
		for (int i = 0; i < _mazeData.config.width; i++) {
			for (int j = 0; j < _mazeData.config.height; j++) {
			
				NodeData node = _mazeData.GetNode (i, j);
				
				
				float[] tileRelativePos = new float[2] {
					(float)i / _mazeData.config.width,
					(float)j / _mazeData.config.height
								};
								
				//create a tile				
				GameObject tileInstance = (GameObject)Instantiate (Prefabs.TILE);
				float tint = 0.3f + 0.7f * (float)node.score / _mazeData.config.maxScore;
				tileInstance.GetComponent<SpriteRenderer> ().color = ColorComponent.GetColorAt (tileRelativePos, colorComponents, tint);
				_tileInstances.Add (tileInstance);
				AddObject (tileInstance, _genericContainer, i, j, 1);

				//create a wall
				float zOrder = (float)(j - i) / (_mazeData.config.width + _mazeData.config.height);				
				GameObject wallInstance = null;
				if (i > 0 && node.HasWall (NodeData.DIRECTION_LEFT_IDX)) {
					
					if ((j < (_mazeData.config.height - 1) && node.HasWall (NodeData.DIRECTION_UP_IDX))) 
						wallInstance = (GameObject)Instantiate (Prefabs.WALL_NORTH_WEST);
					else
						wallInstance = (GameObject)Instantiate (Prefabs.WALL_WEST);
					
				} else if ((j < (_mazeData.config.height - 1) && node.HasWall (NodeData.DIRECTION_UP_IDX))) 
					wallInstance = (GameObject)Instantiate (Prefabs.WALL_NORTH);
				
				if (wallInstance != null) 
					AddObject (wallInstance, _wallsContainer, i, j, zOrder);
				
								
				if (node.HasFlag (NodeData.SPECIALS_EXIT)) 
					AddObject ((GameObject)Instantiate (Prefabs.EXIT), _genericContainer, i, j, zOrder);

								
				if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_UP)) 
					AddObject ((GameObject)Instantiate (Prefabs.SPEED_UP), _genericContainer, i, j, zOrder);
				
				if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_RIGHT)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
					specInstance.transform.eulerAngles = new Vector3 (0, 0, -90);
					AddObject (specInstance, _genericContainer, i, j, zOrder);
				}
				
				if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_DOWN)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
					specInstance.transform.eulerAngles = new Vector3 (0, 0, 180);
					AddObject (specInstance, _genericContainer, i, j, zOrder);
				}
				
				if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_LEFT)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
					specInstance.transform.eulerAngles = new Vector3 (0, 0, 90);
					AddObject (specInstance, _genericContainer, i, j, zOrder);
				}
				
				if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CW)) 
					AddObject ((GameObject)Instantiate (Prefabs.ROTATOR_CW), _genericContainer, i, j, zOrder);
				
				if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CCW)) 
					AddObject ((GameObject)Instantiate (Prefabs.ROTATOR_CCW), _genericContainer, i, j, zOrder);
				
				if (node.HasFlag (NodeData.SPECIALS_HIDE_WALLS)) 
					AddObject ((GameObject)Instantiate (Prefabs.HIDE), _genericContainer, i, j, zOrder);

				if (node.HasFlag (NodeData.SPECIALS_SHOW_WALLS)) 
					AddObject ((GameObject)Instantiate (Prefabs.SHOW), _genericContainer, i, j, zOrder);
			}
		}
	}

	private void AddObject (GameObject instance, GameObject container, int cellX, int cellY, float zOrder)
	{
		_objectInstances.Add (instance);
		instance.transform.parent = container.transform;
		
		if (cellX <= _prevMaxX && cellY <= _prevMaxY)
			instance.transform.localPosition = new Vector3 (cellX * NODE_SIZE, cellY * NODE_SIZE, zOrder);
		else {
			instance.transform.localPosition = new Vector3 (Mathf.Min (_prevMaxX, cellX) * NODE_SIZE, Mathf.Min (_prevMaxY, cellY) * NODE_SIZE, zOrder);
			instance.transform.DOLocalMove (new Vector3 (cellX * NODE_SIZE, cellY * NODE_SIZE, zOrder), TRANSITION_TIME);
		}
	}
	
	private GameObject CreateContainer (string name)
	{
		GameObject result = new GameObject ();
		result.name = name;
		result.transform.parent = transform;
		result.transform.localPosition = new Vector3 (0, 0, 0);
		return result;
	}	
}

