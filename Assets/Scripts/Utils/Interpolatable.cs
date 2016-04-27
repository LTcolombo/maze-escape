using UnityEngine;

namespace Utils
{
	public class Interpolatable
	{
		float _from;
		float _to;
		float _time;
		float _startTime;
		bool _static;

		public Interpolatable ()
		{
			_from = 0;
			_static = true;
		}

		public void SetValue (float start, float end, float time)
		{
			_from = start;
			_to = end;
			_time = time;
			_startTime = Time.time;
			_static = false;
		}

		public void SetValue (float value)
		{
			_from = value;
			_static = true;
		}

		public void Inc(float value){
			_from += value;
			_to += value;
		}

		public void Dec(float value){
			_from -= value;
			_to -= value;
		}

		public void Freeze(){
			_from = this;
			_static = true;
		}

		public static implicit operator float(Interpolatable value)
		{
			if (value._static) {
				return value._from;
			}

			var progress = (Time.time - value._startTime) / value._time;
			if (progress > 1.0f)
				return value._to;
			return value._from + (value._to - value._from) * progress;
		}

		public static implicit operator int (Interpolatable value){
			return (int)(float)value;
		}

		public static implicit operator uint (Interpolatable value){
			return (uint)(float)value;
		}
	}
}
