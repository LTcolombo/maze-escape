using UnityEngine;

public class DelayedValue
{
	float _start;
	float _end;
	float _time;
	float _startTime;

	public DelayedValue (float start, float end, float time)
	{
		_start = start;
		_end = end;
		_time = time;
		_startTime = Time.time;
	}
	
	public uint GetCurrentValueAsUInt ()
	{
		return (uint)GetCurrentValue ();
	}
	
	public int GetCurrentValueAsInt ()
	{
		return (int)GetCurrentValue ();
	}
	
	public float GetCurrentValue ()
	{
		var progress = (Time.time - _startTime) / _time;
		if (progress > 1.0f)
			return _end;
		return _start + (_end - _start) * progress;
	}
}

