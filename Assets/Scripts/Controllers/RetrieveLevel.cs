using Model;
using UnityEngine;

namespace AssemblyCSharp
{
	public class RetrieveLevel
	{
		public void Execute ()
		{
			LevelModel.Instance().SetNumber(PlayerPrefs.GetInt ("maxlevel", 0) / 2);
		}
	}
}

