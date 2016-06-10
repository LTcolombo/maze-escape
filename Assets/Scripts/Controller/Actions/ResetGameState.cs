using Model;
using Notifications;

namespace Controller
{
	public class ResetGameState:Action
	{
		override public PrefromResult Perform (float delta)
		{
			var game = GameModel.Instance ();
			game.state = GameModel.STATE_INITED;
			game.timeBonus.SetValue (DifficultyModel.Instance ().maxTimeBonus, DifficultyModel.Instance ().minTimeBonus, DifficultyModel.Instance ().bonusTime);
			game.movesLeft.SetValue (MazeModel.Instance ().deadEnds [0].GetDistance () * 2);
		
			MazePaceNotifications.GAME_UPDATED.Dispatch ();

			return PrefromResult.COMPLETED;
		}
	}
}

