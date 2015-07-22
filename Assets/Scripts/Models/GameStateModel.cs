namespace Models{
	public struct GameStateModel 
	{
		public const int STATE_INITED = 0;
		public const int STATE_ACTIVATED = 1;
		public const int STATE_STUCK = 2;
		public const int STATE_ENDED = 3;

		public int levelNumber;
		public int score;
		public int maxScore;
		public uint movesLeft;
		public float timeBonus;
		public int state;
	}
}

