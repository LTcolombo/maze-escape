
using System;
using Utils;
using UnityEngine;

namespace Model
{
	///<summary>
	/// A structure that specifies the params of level
	///</summary>
	public class DifficultyModel
	{
		public static int NODE_SIZE = 128;

		public int number;

		private LevelValueSet _size = new LevelValueSet {
			new LevelValue (0, 3),
			new LevelValue (2, 4),
			new LevelValue (30, 10)
		};

		private LevelValueSet _moveTime = new LevelValueSet {
			new LevelValue (0, 1),
			new LevelValue (5, 0.4f),
			new LevelValue (20, 0.2f)
		};

		private LevelValueSet _speedUpsCount = new LevelValueSet {
			new LevelValue (0, 0),
			new LevelValue (24, 4)
		};

		private LevelValueSet _rotatorsCount = new LevelValueSet {
			new LevelValue (0, 0),
			new LevelValue (4, 1)
		};

		private LevelValueSet _hidersCount = new LevelValueSet {
			new LevelValue (0, 0),
			new LevelValue (2, 1)
		};

		private LevelValueSet _minScore = new LevelValueSet {
			new LevelValue (0, 0),
			new LevelValue (30, 30)
		};

		private LevelValueSet _maxScore = new LevelValueSet {
			new LevelValue (0, 5),
			new LevelValue (30, 35)
		};

		private LevelValueSet _maxTimeBonus = new LevelValueSet {
			new LevelValue (0, 2),
			new LevelValue (20, 8)
		};

		private LevelValueSet _minTimeBonus = new LevelValueSet {
			new LevelValue (0, 1)
		};

		private LevelValueSet _bonusTime = new LevelValueSet {
			new LevelValue (0, 3),
			new LevelValue (5, 2),
			new LevelValue (10, 1)
		};
		private LevelValueSet _scoreDrainTime = new LevelValueSet {
			new LevelValue (0, 1)
		};

		public int hidersCount {
			get { 
				return (int)_hidersCount.GetValueForLevel (number);
			}
		}

		public int size {
			get { 
				return (int)_size.GetValueForLevel (number);
			}
		}

		public float moveTime {
			get { 
				return _moveTime.GetValueForLevel (number);
			}
		}

		public int speedUpsCount {
			get { 
				return (int)_speedUpsCount.GetValueForLevel (number);
			}
		}

		public int rotatorsCount {
			get { 
				return (int)_rotatorsCount.GetValueForLevel (number);
			}
		}

		public int minScore {
			get { 
				return (int)_minScore.GetValueForLevel (number);
			}
		}

		public int maxScore {
			get { 
				return (int)_maxScore.GetValueForLevel (number);
			}
		}

		public float maxTimeBonus {
			get { 
				return _maxTimeBonus.GetValueForLevel (number);
			}
		}

		public float minTimeBonus {
			get { 
				return _minTimeBonus.GetValueForLevel (number);
			}
		}

		public float bonusTime {
			get { 
				return _bonusTime.GetValueForLevel (number);
			}
		}

		public float scoreDrainTime {
			get { 
				return _scoreDrainTime.GetValueForLevel (number);
			}
		}

		private static DifficultyModel _instance;

		public static DifficultyModel Instance ()
		{
			if (_instance == null)
				_instance = new DifficultyModel ();

			return _instance;
		}

		private DifficultyModel ()
		{
		}
	}
}
