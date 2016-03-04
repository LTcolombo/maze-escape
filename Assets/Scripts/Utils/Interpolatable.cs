using UnityEngine;

namespace Utils
{
	public class Interpolatable<T>
	{
		T _from;
		T _to;
		float _time;
		float _startTime;
		bool _static;

		public Interpolatable ()
		{
		}

		public void SetValue (T start, T end, float time)
		{
			_from = start;
			_to = end;
			_time = time;
			_startTime = Time.time;
			_static = false;
		}

		public void SetValue (T value)
		{
			_from = value;
			_static = true;
		}

		public void inc(T value){
//			_from += value;
//			_to += value;
		}

		public void dec(T value){
//			_from -= value;
//			_to -= value;
		}

		public static implicit operator T(Interpolatable<T> value)
		{
			if (value._static) {
				return value._from;
			}

			var progress = (Time.time - value._startTime) / value._time;
			if (progress > 1.0f)
				return value._to;
			return value._from;// + (value._to - value._from) * progress;
		}
	}
}
