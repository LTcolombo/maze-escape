using System;
using Model;
using Notifications;

namespace Controller
{
	public class ExitLevel:Action
	{
		override public PrefromResult Perform(float delta){
			GameModel.Instance().state = GameModel.STATE_ENDED;
			GameModel.Instance().movesLeft.SetValue(GameModel.Instance().movesLeft, 0u, 0.5f);
			var avgScore = (LevelModel.Instance ().minScore + LevelModel.Instance ().maxScore) / 2;
			GameModel.Instance().score.SetValue(GameModel.Instance().score, (int)(GameModel.Instance().score + GameModel.Instance().movesLeft * GameModel.Instance().timeBonus * avgScore), 0.5f);
			GameModel.Instance().levelNumber++;
			MazePaceNotifications.GAME_UPDATED.Dispatch ();

			return PrefromResult.COMPLETED;
		}
	}
}

