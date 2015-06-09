using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MazeView : MonoBehaviour
{
	public static float NODE_SIZE = 128f;
	
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
				tileInstance.GetComponent<SpriteRenderer> ().color = ColorComponent.GetColor (tileRelativePos, colorComponents, tint);
				_tileInstances.Add (tileInstance);
				tileInstance.transform.parent = _genericContainer.transform;
				tileInstance.transform.localPosition = new Vector3 (i * NODE_SIZE, j * NODE_SIZE, 1);

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
					AddObject (wallInstance, _wallsContainer, new Vector3 (i * NODE_SIZE, j * NODE_SIZE, zOrder));
				
								
				if (node.HasFlag (NodeData.SPECIALS_EXIT)) 
					AddObject ((GameObject)Instantiate (Prefabs.EXIT), _genericContainer, new Vector3 (i * NODE_SIZE, j * NODE_SIZE, zOrder));

								
				if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_UP)) 
					AddObject ((GameObject)Instantiate (Prefabs.SPEED_UP), _genericContainer, new Vector3 (i * NODE_SIZE, j * NODE_SIZE, zOrder));
				
				if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_RIGHT)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
					specInstance.transform.eulerAngles = new Vector3 (0, 0, -90);
					AddObject (specInstance, _genericContainer, new Vector3 (i * NODE_SIZE, j * NODE_SIZE, zOrder));
				}
				
				if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_DOWN)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
					specInstance.transform.eulerAngles = new Vector3 (0, 0, 180);
					AddObject (specInstance, _genericContainer, new Vector3 (i * NODE_SIZE, j * NODE_SIZE, zOrder));
				}
				
				if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_LEFT)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
					specInstance.transform.eulerAngles = new Vector3 (0, 0, 90);
					AddObject (specInstance, _genericContainer, new Vector3 (i * NODE_SIZE, j * NODE_SIZE, zOrder));
				}
				
				if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CW)) 
					AddObject ((GameObject)Instantiate (Prefabs.ROTATOR_CW), _genericContainer, new Vector3 (i * NODE_SIZE, j * NODE_SIZE, zOrder));
				
				if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CCW)) 
					AddObject ((GameObject)Instantiate (Prefabs.ROTATOR_CCW), _genericContainer, new Vector3 (i * NODE_SIZE, j * NODE_SIZE, zOrder));
				
				if (node.HasFlag (NodeData.SPECIALS_HIDE_WALLS)) 
					AddObject ((GameObject)Instantiate (Prefabs.SHOW), _genericContainer, new Vector3 (i * NODE_SIZE, j * NODE_SIZE, zOrder));

				if (node.HasFlag (NodeData.SPECIALS_SHOW_WALLS)) 
					AddObject ((GameObject)Instantiate (Prefabs.SHOW), _genericContainer, new Vector3 (i * NODE_SIZE, j * NODE_SIZE, zOrder));
				
			}
		}
	}

	private void AddObject (GameObject instance, GameObject container, Vector3 pos)
	{
		_objectInstances.Add (instance);
		instance.transform.parent = container.transform;
		instance.transform.localPosition = pos;
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

