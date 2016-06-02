using UnityEngine;
using System.Collections;


/**
 * Resizes the orthographic size of the camera to contain provided minimal dimensions
 */
namespace View {
	public class CameraFitter : MonoBehaviour
	{
		public int MinWidth;
		public int MinHeight;

		private int _lastWidth = 0;
		private int _lastHeight = 0;

		// Update is called once per frame
		void Update ()
		{
			if (_lastHeight != Screen.height || _lastWidth != Screen.width) 
			{
				float scale = (float)Screen.height/MinHeight;
				float widthShrinkedBy = Mathf.Max(1, scale*(float)MinWidth/Screen.width);
				Camera.main.orthographicSize = widthShrinkedBy * MinHeight/2;
				_lastWidth = Screen.width;
				_lastHeight = Screen.height;
			}
		}
	}
}

