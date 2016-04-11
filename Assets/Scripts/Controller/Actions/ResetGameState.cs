using Model;
using Notifications;

namespace Controller
{
	public class ResetGameState:Action
	{
		override public PrefromResult Perform (float delta)
		{
			GameStateModel.Instance().state = GameStateModel.STATE_INITED;
			GameStateModel.Instance().timeBonus.SetValue (LevelModel.Instance ().maxTimeBonus, LevelModel.Instance ().minTimeBonus, LevelModel.Instance ().bonusTime);
			GameStateModel.Instance().movesLeft.SetValue (MazeModel.Instance().deadEnds [0].GetDistance () * 2);

			MazePaceNotifications.GAME_STATE_UPDATED.Dispatch (GameStateModel.Instance());

			return PrefromResult.COMPLETED;
		}
	}
}

