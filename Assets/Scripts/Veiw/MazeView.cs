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
	
	
		public GameObject tile;
		public GameObject wallWest;
		public GameObject wallNorth;
		public GameObject wallNorthWest;
		private ColorComponent[] colorComponents;
		private MazeData _mazeData;
		private ArrayList _tileInstances = new ArrayList ();
		private ArrayList _wallInstances = new ArrayList ();
		private bool _dirty;
		
		// Use this for initialization
		void Start ()
		{
				Debug.Log ("start!");
				colorComponents = new ColorComponent[3];
				//
				//				colorComponents [0] = new ColorComponent (new Color (0.8f, 0.3f, 0.5f), new float[2] {
				//						0.0f,
				//						0.0f
				//				});
				//
				//				colorComponents [1] = new ColorComponent (new Color (0.4f, 0.7f, 0.3f), new float[2] {
				//			1.0f,
				//			0.5f
				//		});
				//
				//				colorComponents [2] = new ColorComponent (new Color (0.2f, 0.5f, 0.8f), new float[2] {
				//			0.0f,
				//			1.0f
				//		});
		
		
				colorComponents [0] = new ColorComponent (new Color (1.0f, 0.0f, 0.0f), new float[2] {
			0.0f,
			0.0f
		});
		
				colorComponents [1] = new ColorComponent (new Color (0.0f, 1.0f, 0.0f), new float[2] {
			1.0f,
			0.5f
		});
		
				colorComponents [2] = new ColorComponent (new Color (0.0f, 0.0f, 1.0f), new float[2] {
			0.0f,
			1.0f
		});
		}
		
		public void UpdateMazeData (MazeData mazeData)
		{
				_mazeData = mazeData;
				_dirty = true;
		}
		
		private void Redraw ()
		{
		
				foreach (Object wallInstance in _wallInstances)
						Destroy (wallInstance);
						
				_wallInstances.Clear ();
				
		
				foreach (Object tileInstance in _tileInstances)
						Destroy (tileInstance);
		
				_tileInstances.Clear ();
		
				float nodeSize = 0.63f;//Mathf.Min (2 * cameraSize / (_maze.width), 2 * cameraSize / (_maze.height));
				float offsetX = _mazeData.width * nodeSize / 2;
				float offsetY = _mazeData.height * nodeSize / 2;
		
				for (int i = 0; i < _mazeData.width; i++) {
						for (int j = 0; j < _mazeData.height; j++) {
								NodeData node = _mazeData.GetNode (i, j);
				
				
								float[] tileRelativePos = new float[2] {
					(float)i / _mazeData.width,
					(float)j / _mazeData.height
								};
				
								float r = 0;
								float g = 0;
								float b = 0;
				
								for (int colorIdx = 0; colorIdx < 3; colorIdx++) {
										ColorComponent colorComponent = colorComponents [colorIdx];
										float distance = (Mathf.Abs (tileRelativePos [0] - colorComponent.position [0]) + Mathf.Abs (tileRelativePos [1] - colorComponent.position [1])) / 2;
					
										r += colorComponent.color.r - (colorComponent.color.r * distance);			
										g += colorComponent.color.g - (colorComponent.color.g * distance);
										b += colorComponent.color.b - (colorComponent.color.b * distance);
								}
				
				
								float zOrder = (float)(j - i) / (_mazeData.width + _mazeData.height);
				
								Object wallInstance = null;
								if ((i > 0 && (node.walls & NodeData.WEST_WALL) > 0)) {
					
										if ((j < (_mazeData.height - 1) && (node.walls & NodeData.NORTH_WALL) > 0)) 
												wallInstance = Instantiate (wallNorthWest, new Vector3 ((i + 0.5f) * nodeSize - offsetX, (j + 0.5f) * nodeSize - offsetY, zOrder), Quaternion.identity);
										else
												wallInstance = Instantiate (wallWest, new Vector3 ((i + 0.5f) * nodeSize - offsetX, (j + 0.5f) * nodeSize - offsetY, zOrder), Quaternion.identity);
					
								} else if ((j < (_mazeData.height - 1) && (node.walls & NodeData.NORTH_WALL) > 0)) 
										wallInstance = Instantiate (wallNorth, new Vector3 ((i + 0.5f) * nodeSize - offsetX, (j + 0.5f) * nodeSize - offsetY, zOrder), Quaternion.identity);
				
								if (wallInstance != null)
										_wallInstances.Add (wallInstance);
				
								GameObject tileInstance = (GameObject)Instantiate (tile, new Vector3 ((i + 0.5f) * nodeSize - offsetX, (j + 0.5f) * nodeSize - offsetY, 1), Quaternion.identity);
								tileInstance.GetComponent<SpriteRenderer> ().color = new Color ((1 - 0.1f * (1 + node.speed)) * r, (1 - 0.08f * (1 + node.speed)) * g, (1 - 0.08f * (1 + node.speed)) * b, 1);
								_tileInstances.Add (tileInstance);
						}
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (_dirty) {
						Redraw ();
						_dirty = false;
				}
		}
}

