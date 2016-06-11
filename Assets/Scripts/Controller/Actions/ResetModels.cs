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
			DifficultyModel.Instance ().number = 1;//PlayerPrefs.GetInt ("maxlevel", 0) / 2;
			GameModel.Instance ().maxScore.SetValue(PlayerPrefs.GetInt ("highscore", 0));

			GameModel.Instance ().score.SetValue(0);

			PlayerModel.Instance ().cellPosition.x = 0;
			PlayerModel.Instance ().cellPosition.y = 0;

			return PrefromResult.COMPLETED;
		}
	}
}

