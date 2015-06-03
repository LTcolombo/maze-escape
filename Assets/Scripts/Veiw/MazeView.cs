using UnityEngine;
using System.Collections;
using AssemblyCSharp;

class ColorComponent
{
	public Color color;
	public float[] position;
	
	public ColorComponent (Color color, float[] position)
	{
		this.color = color;
		this.position = position;
	}
}

public class MazeView : MonoBehaviour
{
	public static float NODE_SIZE = 128f;
	
	//containers
	private GameObject _wallsContainer;
	private GameObject _genericContainer;
	
	//set of base colors
	private ColorComponent[] _colorComponents;
	
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
	
		_colorComponents = new ColorComponent[3];
		
		_colorComponents [0] = new ColorComponent (new Color (0.7f, 0.3f, 0.0f), new float[2] {
			0.0f,
			0.0f
		});
		
		_colorComponents [1] = new ColorComponent (new Color (0.0f, 0.7f, 0.3f), new float[2] {
			1.0f,
			0.5f
		});
		
		_colorComponents [2] = new ColorComponent (new Color (0.3f, 0.0f, 0.7f), new float[2] {
			0.0f,
			1.0f
		});
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
				
		for (int i = 0; i < _mazeData.config.width; i++) {
			for (int j = 0; j < _mazeData.config.height; j++) {
				NodeData node = _mazeData.GetNode (i, j);
				
				
				float[] tileRelativePos = new float[2] {
					(float)i / _mazeData.config.width,
					(float)j / _mazeData.config.height
								};
				
				float r = 0;
				float g = 0;
				float b = 0;
				
				for (int colorIdx = 0; colorIdx < 3; colorIdx++) {
					ColorComponent colorComponent = _colorComponents [colorIdx];
					float distance = (Mathf.Abs (tileRelativePos [0] - colorComponent.position [0]) + Mathf.Abs (tileRelativePos [1] - colorComponent.position [1])) / 2;
					
					r += colorComponent.color.r - (colorComponent.color.r * distance);			
					g += colorComponent.color.g - (colorComponent.color.g * distance);
					b += colorComponent.color.b - (colorComponent.color.b * distance);
				}
				
				
				float zOrder = (float)(j - i) / (_mazeData.config.width + _mazeData.config.height);
				
				GameObject wallInstance = null;
				if (i > 0 && node.HasWall (NodeData.DIRECTION_LEFT_IDX)) {
					
					if ((j < (_mazeData.config.height - 1) && node.HasWall (NodeData.DIRECTION_UP_IDX))) 
						wallInstance = (GameObject)Instantiate (Prefabs.WALL_NORTH_WEST);
					else
						wallInstance = (GameObject)Instantiate (Prefabs.WALL_WEST);
					
				} else if ((j < (_mazeData.config.height - 1) && node.HasWall (NodeData.DIRECTION_UP_IDX))) 
					wallInstance = (GameObject)Instantiate (Prefabs.WALL_NORTH);
				
				if (wallInstance != null) {
					_objectInstances.Add (wallInstance);
					wallInstance.transform.parent = _wallsContainer.transform;
					wallInstance.transform.localPosition = new Vector3 (i * NODE_SIZE, j * NODE_SIZE, zOrder);
				}
				
				GameObject tileInstance = (GameObject)Instantiate (Prefabs.TILE);
				float tint = 0.3f + 0.7f * (float)node.score / _mazeData.config.maxScore;
				tileInstance.GetComponent<SpriteRenderer> ().color = new Color (tint * r, tint * g, tint * b, 1);
				_tileInstances.Add (tileInstance);
				tileInstance.transform.parent = _genericContainer.transform;
				tileInstance.transform.localPosition = new Vector3 (i * NODE_SIZE, j * NODE_SIZE, 1);
								
				if (node.HasFlag (NodeData.SPECIALS_EXIT)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.EXIT);
					_objectInstances.Add (specInstance);
					specInstance.transform.parent = _genericContainer.transform;
					specInstance.transform.localPosition = new Vector3 (i * NODE_SIZE, j * NODE_SIZE, 0.5f);
				}
								
				if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_UP)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
					_objectInstances.Add (specInstance);
					specInstance.transform.parent = _genericContainer.transform;
					specInstance.transform.localPosition = new Vector3 (i * NODE_SIZE, j * NODE_SIZE, 0.5f);
				}
				
				if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_RIGHT)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
					_objectInstances.Add (specInstance);
					specInstance.transform.parent = _genericContainer.transform;
					specInstance.transform.eulerAngles = new Vector3 (0, 0, -90);
					specInstance.transform.localPosition = new Vector3 (i * NODE_SIZE, j * NODE_SIZE, 0.5f);
				}
				
				if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_DOWN)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
					_objectInstances.Add (specInstance);
					specInstance.transform.parent = _genericContainer.transform;
					specInstance.transform.eulerAngles = new Vector3 (0, 0, 180);
					specInstance.transform.localPosition = new Vector3 (i * NODE_SIZE, j * NODE_SIZE, 0.5f);
				}
				
				if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_LEFT)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
					_objectInstances.Add (specInstance);
					specInstance.transform.parent = _genericContainer.transform;
					specInstance.transform.eulerAngles = new Vector3 (0, 0, 90);
					specInstance.transform.localPosition = new Vector3 (i * NODE_SIZE, j * NODE_SIZE, 0.5f);
				}
				
				if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CW)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.ROTATOR_CW);
					_objectInstances.Add (specInstance);
					specInstance.transform.parent = _genericContainer.transform;
					specInstance.transform.localPosition = new Vector3 (i * NODE_SIZE, j * NODE_SIZE, 0.5f);
				}
				
				if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CCW)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.ROTATOR_CCW);
					_objectInstances.Add (specInstance);
					specInstance.transform.parent = _genericContainer.transform;
					specInstance.transform.localPosition = new Vector3 (i * NODE_SIZE, j * NODE_SIZE, 0.5f);
				}
				
				if (node.HasFlag (NodeData.SPECIALS_HIDE_WALLS)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.HIDE);
					_objectInstances.Add (specInstance);
					specInstance.transform.parent = _genericContainer.transform;
					specInstance.transform.localPosition = new Vector3 (i * NODE_SIZE, j * NODE_SIZE, 0.5f);
				}
				
				if (node.HasFlag (NodeData.SPECIALS_SHOW_WALLS)) {
					GameObject specInstance = (GameObject)Instantiate (Prefabs.SHOW);
					_objectInstances.Add (specInstance);
					specInstance.transform.parent = _genericContainer.transform;
					specInstance.transform.localPosition = new Vector3 (i * NODE_SIZE, j * NODE_SIZE, 0.5f);
				}
			}
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

