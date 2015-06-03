using UnityEngine;

namespace AssemblyCSharp
{
	public class Prefabs
	{
	
		public static GameObject TILE;
		public static GameObject WALL_WEST;
		public static GameObject WALL_NORTH;
		public static GameObject WALL_NORTH_WEST;
		public static GameObject EXIT;
		public static GameObject SPEED_UP;
		public static GameObject ROTATOR_CW;
		public static GameObject ROTATOR_CCW;
		public static GameObject HIDE;
		public static GameObject SHOW;
		public static GameObject MAZE;
		public static GameObject PLAYER;
		public static GameObject TILE_SMALL;
		static bool _inited;

		// Use this for initialization
		public static void Init ()
		{
			if (_inited)
				return;
	
			_inited = true;
		
		
			TILE = (GameObject)(Resources.Load ("Prefabs/Tile"));
			WALL_WEST = (GameObject)(Resources.Load ("Prefabs/WallWest"));
			WALL_NORTH = (GameObject)(Resources.Load ("Prefabs/WallNorth"));
			WALL_NORTH_WEST = (GameObject)(Resources.Load ("Prefabs/WallNorthWest"));
			EXIT = (GameObject)(Resources.Load ("Prefabs/Exit"));
			SPEED_UP = (GameObject)(Resources.Load ("Prefabs/SpeedUp"));
			ROTATOR_CW = (GameObject)(Resources.Load ("Prefabs/RotatorCW"));
			ROTATOR_CCW = (GameObject)(Resources.Load ("Prefabs/RotatorCCW"));
			HIDE = (GameObject)(Resources.Load ("Prefabs/Hide"));
			SHOW = (GameObject)(Resources.Load ("Prefabs/Show"));
			MAZE = (GameObject)(Resources.Load ("Prefabs/Maze"));
			PLAYER = (GameObject)(Resources.Load ("Prefabs/Player"));			
			TILE_SMALL = (GameObject)(Resources.Load ("Prefabs/TileSmall"));
		}
	}	
}

