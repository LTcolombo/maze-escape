
using System;
using Model.Data;

namespace Model
{
	///<summary>
	/// A structure that specifies state of the player
	///</summary>
	public class PlayerModel
	{
		public IntPoint cellPosition;
		public int directionIdx;

		private static PlayerModel _instance;

		public static PlayerModel Instance ()
		{
			if (_instance == null)
				_instance = new PlayerModel ();

			return _instance;
		}

		//move to model
		public PlayerModel (){
			cellPosition = new IntPoint (0, 0);
			directionIdx = NodeVO.DIRECTION_UP_IDX;
		}
	}
}
