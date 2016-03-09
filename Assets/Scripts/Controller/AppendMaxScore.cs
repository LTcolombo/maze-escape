using System;
using Model;
using UnityEngine;

namespace Controller
{
	public class AppendMaxScore
	{
		public void Execute ()
		{
			if (GameStateModel.Instance().maxScore > PlayerPrefs.GetInt ("highscore", 0))
				PlayerPrefs.SetInt ("highscore", GameStateModel.Instance().maxScore);
		}
	}
}

