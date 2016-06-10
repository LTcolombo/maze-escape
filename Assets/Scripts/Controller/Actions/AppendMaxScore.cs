using System;
using Model;
using UnityEngine;

namespace Controller
{
	public class AppendMaxScore:Action
	{
		override public PrefromResult Perform (float delta)
		{
			if ((int)GameModel.Instance().maxScore > PlayerPrefs.GetInt ("highscore", 0))
				PlayerPrefs.SetInt ("highscore", (int)GameModel.Instance().maxScore);

			return PrefromResult.COMPLETED;
		}
	}
}

