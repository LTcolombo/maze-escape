using Notifications.Base;
using Models;
using Models.Data;

namespace Notifications {
	
	public class GameUpdated : Notification1<GameStateModel> {}
	public class MazeUpdated : Notification1<MazeModel> {}
	public class Proceed : Notification2<NodeModel, float> {}
	public class ReadyToProceed : Notification2<IntPoint, int> {}
	public class PlayerStuck : Notification0 {}
	public class ExitReached : Notification0 {}
	public class PlayerDirectionUpdated : Notification1<int> {}

	public class NotificationManager
	{
		public static GameUpdated GAME_STATE_UPDATED = new GameUpdated();
		public static MazeUpdated MAZE_DATA_UPDATED = new MazeUpdated();
		public static Proceed PROCEED = new Proceed();
		public static ReadyToProceed PLAYER_READY_TO_PROCEED = new ReadyToProceed();
		public static PlayerStuck PLAYER_STUCK = new PlayerStuck();
		public static PlayerDirectionUpdated PLAYER_DIRECTION_UPDATED = new PlayerDirectionUpdated();
		public static ExitReached EXIT_REACHED = new ExitReached();

	}
}
