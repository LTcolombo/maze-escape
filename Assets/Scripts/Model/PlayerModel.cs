
using System;
using Model.Data;

namespace Model
{
	///<summary>
	/// A structure that specifies state of the player
	///</summary>
	public class PlayerModel
	{
		public IntPointVO cellPosition;
		public bool moved;

		public int directionIdx { 
			get { 
				return _directionIdx; 
			} 
			set { 
				_directionIdx = value; 

				var directionCount = NodeVO.DIRECTIONS.GetLength (0);
				if (_directionIdx >= directionCount)
					_directionIdx = 0;
								
				if (_directionIdx < 0)
					_directionIdx = directionCount - 1;
			} 
		}

		private int _directionIdx;

		private static PlayerModel _instance;

		public static PlayerModel Instance ()
		{
			if (_instance == null)
				_instance = new PlayerModel ();

			return _instance;
		}

		public float getMoveTime (NodeVO node)
		{
			float moveTime = DifficultyModel.Instance ().moveTime;
			if (node.HasFlag (NodeVO.SPECIALS_SPEEDUP_UP)) {
				if (directionIdx == NodeVO.DIRECTION_UP_IDX)
					moveTime /= 2;

				if (directionIdx == NodeVO.DIRECTION_DOWN_IDX)
					moveTime *= 2;
			}

			if (node.HasFlag (NodeVO.SPECIALS_SPEEDUP_RIGHT)) {
				if (directionIdx == NodeVO.DIRECTION_RIGHT_IDX)
					moveTime /= 2;

				if (directionIdx == NodeVO.DIRECTION_LEFT_IDX)
					moveTime *= 2;
			}

			if (node.HasFlag (NodeVO.SPECIALS_SPEEDUP_DOWN)) {
				if (directionIdx == NodeVO.DIRECTION_DOWN_IDX)
					moveTime /= 2;

				if (directionIdx == NodeVO.DIRECTION_UP_IDX)
					moveTime *= 2;
			}

			if (node.HasFlag (NodeVO.SPECIALS_SPEEDUP_LEFT)) {
				if (directionIdx == NodeVO.DIRECTION_LEFT_IDX)
					moveTime /= 2;

				if (directionIdx == NodeVO.DIRECTION_RIGHT_IDX)
					moveTime *= 2;
			}
			return moveTime;
		}

		private PlayerModel ()
		{
			cellPosition = new IntPointVO (0, 0);
			_directionIdx = NodeVO.DIRECTION_UP_IDX;
		}
	}
}
