using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Animated textfield
 */

[AddComponentMenu( "Utilities/LerpTextField")]
public class LerpTextField : MonoBehaviour
{
	public Color color {
		set {if (_target != null) _target.color = value;}
	}

	public float lerpTime = 0.5f;
	public string prefix;
	public string format = "F0";
	public bool hideOnZero = false;
	private Text _target;
	private float _value;
	private float _currentValue = 0;
	private float _delta;
	private float _currentLerpTime;
	
	void Start ()
	{
		_target = GetComponent<Text> ();
		
		if (_target == null)
			Debug.LogError ("LerpTextField is assigned to a non text object");
	}
	
	public float GetImmediateValue ()
	{
		return _currentValue;
	}
	
	public void SetValueImmediate (float value)
	{
		_currentValue = value;
		_value = value;
		_target.text = prefix + _currentValue.ToString (format);
		
		if (hideOnZero && _currentValue == 0)
			_target.text = "";
	}
	
	public void SetValue (float value)
	{
		_currentLerpTime = lerpTime;
		_delta = value - _currentValue;
		_value = value;
	}
	
	public void SetValue (float value, float time)
	{
		_currentLerpTime = time;
		_delta = value - _currentValue;
		_value = value;
	}
	
	void Update ()
	{
		if (_target == null)
			return;
			
		if (_currentValue == _value)
			return;
			
		if (hideOnZero && _currentValue == 0)
			_target.text = "";
		
		if (_delta * (_value - _currentValue) < 0)
			_currentValue = _value;
		else 
			_currentValue += _delta * Time.deltaTime / _currentLerpTime;
		
		_target.text = prefix + _currentValue.ToString (format);
	}
}

