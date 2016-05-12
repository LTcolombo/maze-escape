using System;
using UnityEngine;

namespace Model
{
	public class LivesModel
	{
		private static int MAX_LIVES = 5;
		private static int RESTORE_TIME = 60;
		private static int TICKS_IN_SECOND = 10000000;

		private long _timeStamp;
		private static LivesModel _instance;

		private LivesModel ()
		{
			string stored = PlayerPrefs.GetString ("timeStamp");
			if (!Int64.TryParse (stored, out _timeStamp)) {
				Debug.Log("cant parse lives, stored value = " + stored);
				_timeStamp = 0;
			}
		}

		public static LivesModel Instance ()
		{
			if (_instance == null)
				_instance = new LivesModel ();

			return _instance;
		}

		public int GetLives ()
		{
			long delta = _timeStamp - DateTime.UtcNow.Ticks;
			if (delta > 0)
				return MAX_LIVES - (int)(delta / (RESTORE_TIME * TICKS_IN_SECOND)) - 1;
			return MAX_LIVES;
		}

		public int NextLifeIn ()
		{
			long delta = _timeStamp - DateTime.UtcNow.Ticks;
			if (delta > 0)
				return (int)(delta / TICKS_IN_SECOND) % RESTORE_TIME + 1;
			return 0;
		}

		public void DecLives ()
		{
			if (_timeStamp < DateTime.UtcNow.Ticks)
				_timeStamp = DateTime.UtcNow.Ticks;
			
			_timeStamp += RESTORE_TIME * TICKS_IN_SECOND;
			PlayerPrefs.SetString ("timeStamp", _timeStamp.ToString());
		}
	}
}

