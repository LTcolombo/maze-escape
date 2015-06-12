using UnityEngine;
using System.Collections;


/**
 * Resizes the orthographic size of the camera to contain provded minimal dimensions
 */
public class CameraFitter : MonoBehaviour
{
	public int MinWidth;
	public int MinHeight;

	private int _lastHeight = 0;

	// Update is called once per frame
	void Update ()
	{
		if (_lastHeight != Screen.height) 
		{
			float scale = (float)Screen.height/MinHeight;
			float widthShrinkedBy = Mathf.Max(1, scale*(float)MinWidth/Screen.width);
			Camera.main.orthographicSize = widthShrinkedBy * MinHeight/2;//
			_lastHeight = Screen.height;
		}
	}
}

