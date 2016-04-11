using System;
using Model;
using UnityEngine;

namespace Controller
{
	public class AppendLevel:Action
	{
		override public PrefromResult Perform (float delta)
		{
			if (GameStateModel.Instance ().levelNumber > PlayerPrefs.GetInt ("maxlevel", 0))
				PlayerPrefs.SetInt ("maxlevel", GameStateModel.Instance ().levelNumber);

			return PrefromResult.COMPLETED;
		}
	}
}

