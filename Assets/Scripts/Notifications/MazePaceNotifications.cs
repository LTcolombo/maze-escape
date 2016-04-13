using Model;
using Model.Data;

namespace Notifications {
	
	public class GameUpdated : Notification0 {}
	public class MazeUpdated : Notification1<MazeModel> {}
	public class Proceed : Notification2<NodeVO, float> {}
	public class ReadyToProceed : Notification2<IntPointVO, int> {}
	public class PlayerStuck : Notification0 {}
	public class ExitReached : Notification0 {}
	public class PlayerDirectionUpdated : Notification1<int> {}
	public class ToggleWallsVisibility : Notification1<bool> {}

	public class MazePaceNotifications
	{
		public static GameUpdated GAME_STATE_UPDATED = new GameUpdated();
		public static MazeUpdated MAZE_RECREATED = new MazeUpdated();
		public static Proceed NODE_REACHED = new Proceed();
		public static ReadyToProceed PLAYER_READY_TO_PROCEED = new ReadyToProceed();
		public static PlayerStuck PLAYER_STUCK = new PlayerStuck();
		public static PlayerDirectionUpdated SET_PLAYER_DIRECTION = new PlayerDirectionUpdated();
		public static ExitReached EXIT_REACHED = new ExitReached();
		public static ToggleWallsVisibility TOGGLE_WALLS_VISIBILITY = new ToggleWallsVisibility();
	}
}
