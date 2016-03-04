using System;
using UnityEngine;
using Model.Data;
using Utils;
using System.Collections.Generic;

namespace View {
	public class NodeController : MonoBehaviour
	{
		SpriteRenderer _tileRenderer;
		GameObject _wallInstance;
		
		//references for cleanup
		List<GameObject> _objects = new List<GameObject> ();
		NodeVO _data;

		// Use this for initialization
		public void Redraw (NodeVO data, Color color)
		{
			_data = data;
			
			foreach (GameObject existingObject in _objects)
				Destroy (existingObject);
			
			_tileRenderer = GetComponent<SpriteRenderer> ();
			_tileRenderer.color = color;

			//create a wall		
			if (_data.pos.x > 0 && _data.HasWall (NodeVO.DIRECTION_LEFT_IDX)) {
								
				if (_data.pos.y > 0 && _data.HasWall (NodeVO.DIRECTION_DOWN_IDX)) 
					_wallInstance = (GameObject)Instantiate (PrefabLib.WALL_SOUTH_WEST);
				else
					_wallInstance = (GameObject)Instantiate (PrefabLib.WALL_WEST);
								
			} else if (_data.pos.y > 0 && _data.HasWall (NodeVO.DIRECTION_DOWN_IDX)) 
				_wallInstance = (GameObject)Instantiate (PrefabLib.WALL_SOUTH);

			if (_wallInstance) {
				_objects.Add (AddObject (_wallInstance));
			}					
											
			if (_data.HasFlag (NodeVO.SPECIALS_EXIT)) 
				_objects.Add (AddObject ((GameObject)Instantiate (PrefabLib.EXIT)));
			
											
			if (_data.HasFlag (NodeVO.SPECIALS_SPEEDUP_UP)) 
				_objects.Add (AddObject ((GameObject)Instantiate (PrefabLib.SPEED_UP)));
							
			if (_data.HasFlag (NodeVO.SPECIALS_SPEEDUP_RIGHT)) {
				GameObject specInstance = (GameObject)Instantiate (PrefabLib.SPEED_UP);
				specInstance.transform.eulerAngles = new Vector3 (0, 0, -90);
				_objects.Add (AddObject (specInstance));
			}
							
			if (_data.HasFlag (NodeVO.SPECIALS_SPEEDUP_DOWN)) {
				GameObject specInstance = (GameObject)Instantiate (PrefabLib.SPEED_UP);
				specInstance.transform.eulerAngles = new Vector3 (0, 0, 180);
				_objects.Add (AddObject (specInstance));
			}
							
			if (_data.HasFlag (NodeVO.SPECIALS_SPEEDUP_LEFT)) {
				GameObject specInstance = (GameObject)Instantiate (PrefabLib.SPEED_UP);
				specInstance.transform.eulerAngles = new Vector3 (0, 0, 90);
				_objects.Add (AddObject (specInstance));
			}
							
			if (_data.HasFlag (NodeVO.SPECIALS_ROTATOR_CW)) 
				_objects.Add (AddObject ((GameObject)Instantiate (PrefabLib.ROTATOR_CW)));
							
			if (_data.HasFlag (NodeVO.SPECIALS_ROTATOR_CCW)) 
				_objects.Add (AddObject ((GameObject)Instantiate (PrefabLib.ROTATOR_CCW)));
							
			if (_data.HasFlag (NodeVO.SPECIALS_HIDE_WALLS)) 
				_objects.Add (AddObject ((GameObject)Instantiate (PrefabLib.HIDE)));
			
			if (_data.HasFlag (NodeVO.SPECIALS_SHOW_WALLS)) 
				_objects.Add (AddObject ((GameObject)Instantiate (PrefabLib.SHOW)));
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

