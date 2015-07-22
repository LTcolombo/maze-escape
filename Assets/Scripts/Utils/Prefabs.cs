using UnityEngine;

namespace Utils
{
	/**
	 * A prefab decpriptors static storage class
	 */

	public class Prefabs
	{
	
		public static GameObject NODE;
		public static GameObject WALL_WEST;
		public static GameObject WALL_SOUTH;
		public static GameObject WALL_SOUTH_WEST;
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
				
			NODE = (GameObject)(Resources.Load ("Prefabs/Node"));
			WALL_WEST = (GameObject)(Resources.Load ("Prefabs/WallWest"));
			WALL_SOUTH = (GameObject)(Resources.Load ("Prefabs/WallSouth"));
			WALL_SOUTH_WEST = (GameObject)(Resources.Load ("Prefabs/WallSouthWest"));
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

