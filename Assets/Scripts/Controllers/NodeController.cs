using System;
using UnityEngine;
using Models;
using Utils;
using System.Collections.Generic;

namespace Controllers {
	public class NodeController : MonoBehaviour
	{
		SpriteRenderer _tileRenderer;
		GameObject _wallInstance;
		
		//references for cleanup
		List<GameObject> _objects = new List<GameObject> ();
		NodeModel _data;

		// Use this for initialization
		public void Redraw (NodeModel data, Color color)
		{
			_data = data;
			
			foreach (GameObject existingObject in _objects)
				Destroy (existingObject);
			
			_tileRenderer = GetComponent<SpriteRenderer> ();
			_tileRenderer.color = color;

			//create a wall		
			if (_data.pos.x > 0 && _data.HasWall (NodeModel.DIRECTION_LEFT_IDX)) {
								
				if (_data.pos.y > 0 && _data.HasWall (NodeModel.DIRECTION_DOWN_IDX)) 
					_wallInstance = (GameObject)Instantiate (Prefabs.WALL_SOUTH_WEST);
				else
					_wallInstance = (GameObject)Instantiate (Prefabs.WALL_WEST);
								
			} else if (_data.pos.y > 0 && _data.HasWall (NodeModel.DIRECTION_DOWN_IDX)) 
				_wallInstance = (GameObject)Instantiate (Prefabs.WALL_SOUTH);

			if (_wallInstance) {
				_objects.Add (AddObject (_wallInstance));
			}					
											
			if (_data.HasFlag (NodeModel.SPECIALS_EXIT)) 
				_objects.Add (AddObject ((GameObject)Instantiate (Prefabs.EXIT)));
			
											
			if (_data.HasFlag (NodeModel.SPECIALS_SPEEDUP_UP)) 
				_objects.Add (AddObject ((GameObject)Instantiate (Prefabs.SPEED_UP)));
							
			if (_data.HasFlag (NodeModel.SPECIALS_SPEEDUP_RIGHT)) {
				GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
				specInstance.transform.eulerAngles = new Vector3 (0, 0, -90);
				_objects.Add (AddObject (specInstance));
			}
							
			if (_data.HasFlag (NodeModel.SPECIALS_SPEEDUP_DOWN)) {
				GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
				specInstance.transform.eulerAngles = new Vector3 (0, 0, 180);
				_objects.Add (AddObject (specInstance));
			}
							
			if (_data.HasFlag (NodeModel.SPECIALS_SPEEDUP_LEFT)) {
				GameObject specInstance = (GameObject)Instantiate (Prefabs.SPEED_UP);
				specInstance.transform.eulerAngles = new Vector3 (0, 0, 90);
				_objects.Add (AddObject (specInstance));
			}
							
			if (_data.HasFlag (NodeModel.SPECIALS_ROTATOR_CW)) 
				_objects.Add (AddObject ((GameObject)Instantiate (Prefabs.ROTATOR_CW)));
							
			if (_data.HasFlag (NodeModel.SPECIALS_ROTATOR_CCW)) 
				_objects.Add (AddObject ((GameObject)Instantiate (Prefabs.ROTATOR_CCW)));
							
			if (_data.HasFlag (NodeModel.SPECIALS_HIDE_WALLS)) 
				_objects.Add (AddObject ((GameObject)Instantiate (Prefabs.HIDE)));
			
			if (_data.HasFlag (NodeModel.SPECIALS_SHOW_WALLS)) 
				_objects.Add (AddObject ((GameObject)Instantiate (Prefabs.SHOW)));
		}

		public void onReached ()
		{		
			//normalize color
			float sumColor = _tileRenderer.color.r + _tileRenderer.color.g + _tileRenderer.color.b; 
			_tileRenderer.color = new Color (sumColor / 3, sumColor / 3, sumColor / 3, 1f);
			
			foreach (GameObject existingObject in _objects) {
				AudioSource audio = existingObject.GetComponent<AudioSource> ();
				if (audio != null)
					audio.Play ();
			}
		}
		
		//move these to specific elements
		public void ShowWall (bool value)
		{
			if (_wallInstance != null)
				_wallInstance.SetActive (value);
		}
		
		GameObject AddObject (GameObject instance)
		{
			instance.transform.localPosition = new Vector3 (0, 0, -0.5f);
			instance.transform.parent = transform;
			
			return instance;
		}
	}
}

