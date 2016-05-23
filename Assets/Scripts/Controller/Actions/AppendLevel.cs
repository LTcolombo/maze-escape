using System;
using Model;
using UnityEngine;

namespace Controller
{
	public class AppendLevel:Action
	{
		override public PrefromResult Perform (float delta)
		{
			if (DifficultyModel.Instance ().number > PlayerPrefs.GetInt ("maxlevel", 0))
				PlayerPrefs.SetInt ("maxlevel", DifficultyModel.Instance ().number);

			return PrefromResult.COMPLETED;
		}
	}
}

