using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Adjust the font to be relative to screen size.
 * Assumes the Y anchor to be 0
 */

namespace View {
	[AddComponentMenu( "Utilities/FontSizeAdjuster")]
	public class ButtonSizeAdjuster : MonoBehaviour
	{
		public float relativeSize;
		public float relativeX;
		public float relativeY;
		private RectTransform _target;
		private int _lastSize;

		void Start ()
		{
			_target = GetComponent<RectTransform> ();
			
			
			if (_target == null)
				Debug.LogError("ButtonSizeAdjuster is assigned to a non RectTransform object");
		}

		void Update ()
		{
			if (_target == null)
				return;
		
			var newSize = (Screen.height);
		
			if (_lastSize != newSize) {
				_target.sizeDelta = new Vector2(relativeSize * newSize, relativeSize * newSize);
				_lastSize = newSize;
				_target.anchoredPosition = new Vector2 (relativeX * newSize, relativeY * newSize);
			}
		}
	}
}

