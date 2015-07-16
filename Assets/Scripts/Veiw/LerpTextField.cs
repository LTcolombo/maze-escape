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

	private AudioSource _audio;
	
	void Start ()
	{
		_target = GetComponent<Text> ();
		
		_audio = GetComponent<AudioSource>();
		
		if (_target == null)
			Debug.LogError ("LerpTextField is assigned to a non text object");
	}
	
	public float GetImmediateValue ()
	{
		return _currentValue;
	}
	
	public void SetValueImmediate (float value)
	{
		if (_audio!=null && _currentValue < value)
			_audio.Play();
	
		_currentValue = value;
		_value = value;
		_target.text = prefix + _currentValue.ToString (format);
		
		if (hideOnZero && _currentValue == 0)
			_target.text = "";
	}
	
	public void SetValue (float value)
	{
		SetValue(value, lerpTime);
	}
	
	public void SetValue (float value, float time)
	{
		_currentLerpTime = time;
		_delta = value - _currentValue;
		_value = value;
		
		if (_audio!=null && _delta>0) {
			_audio.loop = true;
			_audio.Play();
		}
	}
	
	void Update ()
	{
		if (_target == null)
			return;
			
		if (_currentValue == _value)
			return;
			
		if (hideOnZero && _currentValue == 0)
			_target.text = "";
		
		if (_delta * (_value - _currentValue) < 0){
			_currentValue = _value;
			if (_audio!=null) 
				_audio.loop = false;
		} else 
			_currentValue += _delta * Time.deltaTime / _currentLerpTime;
			
		_target.text = prefix + _currentValue.ToString (format);
	}
}

