using System;
using Model;
using UnityEngine;

namespace Controller
{
	public class AppendMaxScore:Action
	{
		override public PrefromResult Perform (float delta)
		{
			if (GameStateModel.Instance().maxScore > PlayerPrefs.GetInt ("highscore", 0))
				PlayerPrefs.SetInt ("highscore", GameStateModel.Instance().maxScore);

			return PrefromResult.COMPLETED;
		}
	}
}

