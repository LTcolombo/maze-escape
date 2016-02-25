using UnityEngine;

namespace Utils
{
	/**
	 * A prefab decpriptors static storage class
	 */
	public class Prefabs
	{
		public static GameObject NODE = (GameObject)(Resources.Load ("Prefabs/Node"));
		public static GameObject WALL_WEST = (GameObject)(Resources.Load ("Prefabs/WallWest"));
		public static GameObject WALL_SOUTH = (GameObject)(Resources.Load ("Prefabs/WallSouth"));
		public static GameObject WALL_SOUTH_WEST = (GameObject)(Resources.Load ("Prefabs/WallSouthWest"));
		public static GameObject EXIT = (GameObject)(Resources.Load ("Prefabs/Exit"));
		public static GameObject SPEED_UP = (GameObject)(Resources.Load ("Prefabs/SpeedUp"));
		public static GameObject ROTATOR_CW = (GameObject)(Resources.Load ("Prefabs/RotatorCW"));
		public static GameObject ROTATOR_CCW = (GameObject)(Resources.Load ("Prefabs/RotatorCCW"));
		public static GameObject HIDE = (GameObject)(Resources.Load ("Prefabs/Hide"));
		public static GameObject SHOW = (GameObject)(Resources.Load ("Prefabs/Show"));
		public static GameObject MAZE = (GameObject)(Resources.Load ("Prefabs/Maze"));
		public static GameObject PLAYER = (GameObject)(Resources.Load ("Prefabs/Player"));			
		public static GameObject TILE_SMALL = (GameObject)(Resources.Load ("Prefabs/TileSmall"));
	}	
}

