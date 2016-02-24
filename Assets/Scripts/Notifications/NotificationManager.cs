using Notifications.Base;
using Models;
using Models.Data;

namespace Notifications {
	
	public class GameUpdated : Notification1<GameStateModel> {}
	public class MazeUpdated : Notification1<MazeModel> {}
	public class NodePassed : Notification1<NodeModel> {}
	public class StepCompleted : Notification1<IntPoint> {}
	public class PlayerStuck : Notification0 {}
	public class PlayerDirectionUpdated : Notification1<int> {}

	public class NotificationManager
	{
		public static GameUpdated GAME_STATE_UPDATED = new GameUpdated();
		public static MazeUpdated MAZE_DATA_UPDATED = new MazeUpdated();
		public static NodePassed NODE_PASSED = new NodePassed();
		public static StepCompleted PLAYER_STEP_COMPLETED = new StepCompleted();
		public static PlayerStuck PLAYER_STUCK = new PlayerStuck();
		public static PlayerDirectionUpdated PLAYER_DIRECTION_UPDATED = new PlayerDirectionUpdated();

	}
}
