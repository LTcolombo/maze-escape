using System;
using UnityEngine;
using AssemblyCSharp;

public class NodeView : MonoBehaviour
{
	private SpriteRenderer _tileRenderer;
	private GameObject _wallInstance;

	// Use this for initialization
	public void Redraw (NodeData node, Color color)
	{
		_tileRenderer = GetComponent<SpriteRenderer> ();
		_tileRenderer.color = color;

		//create a wall		
		if (node.pos.x > 0 && node.HasWall (NodeData.DIRECTION_LEFT_IDX)) {
							
			if (node.pos.y > 0 && node.HasWall (NodeData.DIRECTION_DOWN_IDX)) 
				_wallInstance = (GameObject)Instantiate (Prefabs.WALL_SOUTH_WEST);
			else
				_wallInstance = (GameObject)Instantiate (Prefabs.WALL_WEST);
							
		} else if (node.pos.y > 0 && node.HasWall (NodeData.DIRECTION_DOWN_IDX)) 
			_wallInstance = (GameObject)Instantiate (Prefabs.WALL_SOUTH);

		if (_wallInstance) {
			AddObject(_wallInstance);
		}					
										
		if (node.HasFlag (NodeData.SPECIALS_EXIT)) 
			AddObject ((GameObject)Instantiate (Prefabs.EXIT));
		
										
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_UP)) 
			AddObject ((GameObject)Instantiate (Prefabs.SPEED_UP));
						
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_RIGHT)) {
			GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
			specInstance.transform.eulerAngles = new Vector3 (0, 0, -90);
			AddObject (specInstance);
		}
						
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_DOWN)) {
			GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
			specInstance.transform.eulerAngles = new Vector3 (0, 0, 180);
			AddObject (specInstance);
		}
						
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_LEFT)) {
			GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
			specInstance.transform.eulerAngles = new Vector3 (0, 0, 90);
			AddObject (specInstance);
		}
						
		if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CW)) 
			AddObject ((GameObject)Instantiate (Prefabs.ROTATOR_CW));
						
		if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CCW)) 
			AddObject ((GameObject)Instantiate (Prefabs.ROTATOR_CCW));
						
		if (node.HasFlag (NodeData.SPECIALS_HIDE_WALLS)) 
			AddObject ((GameObject)Instantiate (Prefabs.HIDE));
		
		if (node.HasFlag (NodeData.SPECIALS_SHOW_WALLS)) 
			AddObject ((GameObject)Instantiate (Prefabs.SHOW));
	}

	public void Desaturate ()
	{		
		//normalize color
		float sumColor = _tileRenderer.color.r + _tileRenderer.color.g + _tileRenderer.color.b; 
		_tileRenderer.color = new Color (sumColor / 3, sumColor / 3, sumColor / 3, 1f);
	}

	public void ShowWall (bool value)
	{
		if (_wallInstance != null)
			_wallInstance.transform.localPosition = new Vector3 (0, 0, value ? -0.5f : 0.5f);
	}
	
	private void AddObject (GameObject instance)
	{
		instance.transform.localPosition = new Vector3 (0, 0, -0.5f);
		instance.transform.parent = transform;
	}
}

