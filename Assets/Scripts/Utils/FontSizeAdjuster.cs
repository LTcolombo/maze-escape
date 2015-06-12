using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Adjust the font to be relative to screen size.
 * Assumes the Y anchor to be 0
 */

[AddComponentMenu( "Utilities/FontSizeAdjuster")]
public class FontSizeAdjuster : MonoBehaviour
{
	public float relativeSize;
	public float relativeY;
	private Text _target;
	private int _lastHeight;

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
	
		if (_lastHeight != Screen.height) {
			_target.fontSize = Mathf.CeilToInt (relativeSize * (float)Screen.height);
			_lastHeight = Screen.height;
			_target.rectTransform.anchoredPosition = new Vector2 (0, relativeY * (float)Screen.height);
		}
	}
}

