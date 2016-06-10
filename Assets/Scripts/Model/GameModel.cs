using Utils;
using UnityEngine;

namespace Model
{
	public class GameModel
	{
		public const int STATE_INVALID = -1;
		public const int STATE_INITED = 0;
		public const int STATE_MOVING = 1;
		public const int STATE_STUCK = 2;
		public const int STATE_ENDED = 3;

		public int state;

		public Interpolatable maxScore;
		public Interpolatable score;
		public Interpolatable movesLeft;
		public Interpolatable timeBonus;

		private static GameModel _instance;

		public static GameModel Instance ()
		{
			if (_instance == null)
				_instance = new GameModel ();

			return _instance;
		}

		public void AddScore (int value)
		{
			score.Inc (value);
		
			if (maxScore < score) {
				maxScore.SetValue(score);
			}
		}

		public bool GetIsLost ()
		{ 
			return ((state == GameModel.STATE_STUCK && (int)score == 0)) || movesLeft == 0; 
		}

		public bool IsChanging ()
		{ 
			return state == GameModel.STATE_STUCK || state == GameModel.STATE_ENDED; 
		}

		private GameModel ()
		{
			state = GameModel.STATE_INVALID;
			maxScore = new Interpolatable ();
			score = new Interpolatable ();
			movesLeft = new Interpolatable ();
			timeBonus = new Interpolatable ();
		}
	}
}

