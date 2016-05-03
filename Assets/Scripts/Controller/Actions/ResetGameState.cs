using Model;
using Notifications;

namespace Controller
{
	public class ResetGameState:Action
	{
		override public PrefromResult Perform (float delta)
		{
			GameModel.Instance().state = GameModel.STATE_INITED;
			GameModel.Instance().timeBonus.SetValue (LevelModel.Instance ().maxTimeBonus, LevelModel.Instance ().minTimeBonus, LevelModel.Instance ().bonusTime);
			GameModel.Instance().movesLeft.SetValue (MazeModel.Instance().deadEnds [0].GetDistance () * 2);

			MazePaceNotifications.GAME_UPDATED.Dispatch ();

			return PrefromResult.COMPLETED;
		}
	}
}

