using System;
using Model;
using Notifications;

namespace Controller
{
	public class ExitLevel:Action
	{
		override public PrefromResult Perform(float delta){
			var game = GameModel.Instance ();
			game.movesLeft.SetValue(game.movesLeft, 0u, 1.8f);
			var avgScore = (DifficultyModel.Instance ().minScore + DifficultyModel.Instance ().maxScore) / 2;
			var newScore = (int)(game.score + game.movesLeft * game.timeBonus * avgScore);
			game.score.SetValue (game.score, newScore, 1.8f);
			if (game.maxScore < newScore) {
				game.maxScore.SetValue (game.maxScore, newScore, 1.8f);
			}
			DifficultyModel.Instance ().number++;
			MazePaceNotifications.GAME_UPDATED.Dispatch ();

			return PrefromResult.COMPLETED;
		}
	}
}

