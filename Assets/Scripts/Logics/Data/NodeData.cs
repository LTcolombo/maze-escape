
using System;

namespace AssemblyCSharp
{
	public class NodeData
	{
		
		public const int DIRECTION_UP_IDX = 0;
		public const int DIRECTION_RIGHT_IDX = 1;
		public const int DIRECTION_DOWN_IDX = 2;
		public const int DIRECTION_LEFT_IDX = 3;
				
		//
		public const uint PROCESSED = 1;
				
		//walls
		public const uint UP_WALL = 1 << 1;
		public const uint RIGHT_WALL = 1 << 2;
		public const uint DOWN_WALL = 1 << 3;
		public const uint LEFT_WALL = 1 << 4;
		
		//specials
		public const uint SPECIALS_EXIT = 1 << 5;
		public const uint SPECIALS_SPEEDUP_UP = 1 << 6;
		public const uint SPECIALS_SPEEDUP_RIGHT = 1 << 7;
		public const uint SPECIALS_SPEEDUP_DOWN = 1 << 8;
		public const uint SPECIALS_SPEEDUP_LEFT = 1 << 9;
		public const uint SPECIALS_ROTATOR_CW = 1 << 10;
		public const uint SPECIALS_ROTATOR_CCW = 1 << 11;

		//direction vectors
		public static int[,] DIRECTIONS = {{0, 1}, {1, 0}, {0, -1}, {-1, 0}};
		
		//position on grid
		public int x;
		public int y;
		
		//score assigned to node
		public int score = 0;
		
		//previous node during maze generation
		public NodeData previousNode;

		//node state		
		private uint _data = UP_WALL | RIGHT_WALL | DOWN_WALL | LEFT_WALL;

		public NodeData (int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public void RemoveWall (uint direction_idx)
		{
			if (direction_idx == DIRECTION_UP_IDX)
				_data &= ~UP_WALL;
			else if (direction_idx == DIRECTION_RIGHT_IDX)
				_data &= ~RIGHT_WALL;
			else if (direction_idx == DIRECTION_DOWN_IDX)
				_data &= ~DOWN_WALL;
			else if (direction_idx == DIRECTION_LEFT_IDX)
				_data &= ~LEFT_WALL;
		}

		public bool HasWall (int direction_idx)
		{
			if (direction_idx == DIRECTION_UP_IDX)
				return (_data & UP_WALL) > 0;
			else if (direction_idx == DIRECTION_RIGHT_IDX)
				return (_data & RIGHT_WALL) > 0;
			else if (direction_idx == DIRECTION_DOWN_IDX)
				return (_data & DOWN_WALL) > 0;
			else if (direction_idx == DIRECTION_LEFT_IDX)
				return (_data & LEFT_WALL) > 0;
			
			return false;
		}
				
		public void AddFlag (uint type)
		{
			_data |= type;
		}
				
		public bool HasFlag (uint type)
		{
			return (_data & type) > 0;
		}

		public uint MatchCount (uint compared)
		{
			uint temp = _data & compared;

			uint count = 0;
			for (var i = 0; i < sizeof(uint)*8; i++) {
				if ((temp & 1 << i)>0)
					count ++;
			}

			return count;
		}
		
		public uint GetDistance ()
		{
			uint distance = 0;
			NodeData node = previousNode;
			while (node != null) {
				distance++;
				node = node.previousNode;
			}
			
			return distance;
		}
	}
}

