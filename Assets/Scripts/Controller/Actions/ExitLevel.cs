using System;
using Model;
using Notifications;

namespace Controller
{
	public class ExitLevel:Action
	{
		override public PrefromResult Perform(float delta){
			GameStateModel.Instance().state = GameStateModel.STATE_ENDED;
			GameStateModel.Instance().movesLeft.SetValue(GameStateModel.Instance().movesLeft, 0u, 0.5f);
			var avgScore = (LevelModel.Instance ().minScore + LevelModel.Instance ().maxScore) / 2;
			GameStateModel.Instance().score.SetValue(GameStateModel.Instance().score, (int)(GameStateModel.Instance().score + GameStateModel.Instance().movesLeft * GameStateModel.Instance().timeBonus * avgScore), 0.5f);
			GameStateModel.Instance().levelNumber++;
			MazePaceNotifications.GAME_STATE_UPDATED.Dispatch (GameStateModel.Instance());

			MazePaceNotifications.EXIT_REACHED.Dispatch ();

			return PrefromResult.COMPLETED;
		}
	}
}

