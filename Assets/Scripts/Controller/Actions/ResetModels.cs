using Model;
using Notifications;
using UnityEngine;

namespace Controller
{
	public class ResetModels:Action
	{
		override public PrefromResult Perform (float delta)
		{
			//todo persistent data provider?
			LevelModel.Instance().SetNumber(PlayerPrefs.GetInt ("maxlevel", 0) / 2);

			PlayerModel.Instance ().cellPosition.x = 0;
			PlayerModel.Instance ().cellPosition.y = 0;

			return PrefromResult.COMPLETED;
		}
	}
}

