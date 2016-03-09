using Model;
using UnityEngine;

namespace Controller
{
	public class RetrieveLevel
	{
		public void Execute ()
		{
			LevelModel.Instance().SetNumber(PlayerPrefs.GetInt ("maxlevel", 0) / 2);
		}
	}
}

