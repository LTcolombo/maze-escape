using System;
using Model;
using UnityEngine;

namespace Controller
{
	public class AppendLevel
	{
		public void Execute ()
		{
			if (GameStateModel.Instance().levelNumber > PlayerPrefs.GetInt ("maxlevel", 0))
				PlayerPrefs.SetInt ("maxlevel", GameStateModel.Instance().levelNumber);
		}
	}
}

