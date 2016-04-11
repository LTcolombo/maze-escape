using Utils;

namespace Model
{
	public class GameStateModel
	{
		public const int STATE_INVALID = -1;
		public const int STATE_INITED = 0;
		public const int STATE_ACTIVATED = 1;
		public const int STATE_STUCK = 2;
		public const int STATE_ENDED = 3;

		public int levelNumber;
		public int maxScore;
		public int state;

		public Interpolatable score;
		public Interpolatable movesLeft;
		public Interpolatable timeBonus;

		private static GameStateModel _instance;

		public static GameStateModel Instance ()
		{
			if (_instance == null)
				_instance = new GameStateModel ();

			return _instance;
		}

		private GameStateModel ()
		{
			levelNumber = 0;
			maxScore = 0;
			state = GameStateModel.STATE_INVALID;
			score = new Interpolatable ();
			movesLeft = new Interpolatable ();
			timeBonus = new Interpolatable ();
		}
	}
}

