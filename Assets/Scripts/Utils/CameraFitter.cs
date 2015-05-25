using UnityEngine;
using System.Collections;

public class CameraFitter : MonoBehaviour
{

		private int _lastHeight = 0;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (_lastHeight != Screen.height) {
						Camera.main.orthographicSize = Screen.height;
						_lastHeight = Screen.height;
				}
		}
}

