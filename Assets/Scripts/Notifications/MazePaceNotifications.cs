using Model;
using Model.Data;

namespace Notifications {
	
	public class GameUpdated : Notification0 {}
	public class MazeUpdated : Notification1<MazeModel> {}
	public class NodeReached : Notification0 {}
	public class ProceedFromNode : Notification1<NodeVO> {}
	public class RotateAtNode : Notification1<NodeVO> {}
	public class ReadyToProceed : Notification2<IntPointVO, int> {}
	public class PlayerStuck : Notification0 {}
	public class ExitReached : Notification0 {}
	public class DirectionUpdated : Notification1<int> {}
	public class ToggleWallsVisibility : Notification1<bool> {}

	public class MazePaceNotifications
	{
		public static GameUpdated GAME_UPDATED = new GameUpdated();
		public static MazeUpdated MAZE_RECREATED = new MazeUpdated();
		public static NodeReached NODE_REACHED = new NodeReached();
		public static ProceedFromNode PROCEED_FROM_NODE = new ProceedFromNode();
		public static RotateAtNode ROTATE_AT_NODE = new RotateAtNode();
		public static PlayerStuck PLAYER_STUCK = new PlayerStuck();
		public static DirectionUpdated DIRECTION_UPDATED = new DirectionUpdated();
		public static ExitReached EXIT_REACHED = new ExitReached();
		public static ToggleWallsVisibility TOGGLE_WALLS_VISIBILITY = new ToggleWallsVisibility();
	}
}
