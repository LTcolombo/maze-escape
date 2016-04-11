using Model;
using UnityEngine;

namespace Controller
{
	public class RetrieveLevel:Action
	{
		override public PrefromResult Perform (float delta)
		{
			LevelModel.Instance().SetNumber(PlayerPrefs.GetInt ("maxlevel", 0) / 2);

			return PrefromResult.COMPLETED;
		}
	}
}

