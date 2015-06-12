
namespace AssemblyCSharp
{
	///<summary>
	/// A structure that specifies the params of level
	///</summary>
	public class MazeConfig
	{
		public int width;
		public int height;
		public int speedUpsCount = 0;
		public int rotatorsCount = 0;
		public int hidersCount = 0;
		public int minScore;
		public int maxScore;
		public float bonusRate = 2;
		public float moveTime = 0.5f;
		
		//move to model
		public MazeConfig (int levelNumber)
		{			
			int size = 3 + levelNumber/4;
			
			if (size > 10)
				size = 10;
			
			width = size;
			height = size;
			
			moveTime = 0.5f - 0.01f * System.Math.Min (30, levelNumber);
			
			minScore = 1 + levelNumber;
			maxScore = 4 + levelNumber;
			
			speedUpsCount = levelNumber / 5;
			rotatorsCount = levelNumber / 10;
			hidersCount = levelNumber > 3 ? 1 : 0;
		}
	}
}
