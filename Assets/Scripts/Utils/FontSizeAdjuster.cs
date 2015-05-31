using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu( "Utilities/FontSizeAdjuster")]
public class FontSizeAdjuster : MonoBehaviour
{

	public float percentage;
	private Text _target;
	private int _lastHeight;

	// Use this for initialization
	void Start ()
	{
		_target = GetComponent<Text> ();
		
		
		if (_target == null)
			Debug.LogError("FontSizeAdjuster is assigned to a non text object");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_target == null)
			return;
	
		if (_lastHeight != Screen.height) {
			_target.fontSize = Mathf.CeilToInt (percentage * (float)Screen.height);
			_lastHeight = Screen.height;
			int sign = (_target.rectTransform.anchoredPosition.y > 0) ? 1 : -1;
			_target.rectTransform.anchoredPosition = new Vector2 (0, sign * (float)_target.fontSize * 0.9f);
		}
	}
}

