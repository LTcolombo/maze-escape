using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Adjust the font to be relative to screen size.
 */

namespace View {
	public class FontSizeAdjuster : MonoBehaviour
	{
		public float relativeSize;
		public float relativeY;
		private Text _target;
		private int _lastSize;

		void Start ()
		{
			_target = GetComponent<Text> ();
			
			
			if (_target == null)
				Debug.LogError("FontSizeAdjuster is assigned to a non text object");
		}

		void Update ()
		{
			if (_target == null)
				return;

			var newSize = (Screen.height);
			if (_lastSize != newSize) 
			{
				_target.fontSize = Mathf.CeilToInt (relativeSize * (float)newSize);
				_target.rectTransform.anchoredPosition = new Vector2 (0, relativeY * (float)newSize);
				_lastSize = newSize;
			}
		}
	}
}

