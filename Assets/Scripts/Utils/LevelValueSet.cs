using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
	struct LevelValue
	{
		public int level { get { return _level; } }

		public float value { get { return _value; } }

		int _level;
		float _value;

		public LevelValue (int level, float value)
		{
			_level = level;
			_value = value;
		}
	}

	class LevelValueSet : List<LevelValue>
	{
		public float GetValueForLevel (int level)
		{
			int i = 0;
			for (i = 0; i < Count; i++) {
				if (this [i].level == level)
					return this [i].value;
				if (this [i].level > level) {
					if (i == 0)
						return 	this [i].value;
					else {
						float deltaValue = this [i].value - this [i - 1].value;
						float deltaLevel = this [i].level - this [i - 1].level;
						return this [i-1].value + deltaValue * ((float)(level - this [i - 1].level) / deltaLevel);
					}
				}
			}
			return this [Count-1].value;
		}
	}
}

