
namespace AssemblyCSharp
{
	///<summary>
	/// A structure that specifies the params of level
	///</summary>
	public class MazeConfig
	{
		public int width;
		public int height;
		public int speedUpsCount;
		public int rotatorsCount;
		public int hidersCount;
		public int minScore;
		public int maxScore;
		public float maxTimeBonus;
		public float minTimeBonus;
		public float bonusTime;
		public float moveTime;
		
		//move to model
		public MazeConfig (int levelNumber)
		{			
			int size = 3 + levelNumber/4;
			
			if (size > 10)
				size = 10;
			
			width = size;
			height = size;
			
			moveTime = 0.5f - 0.01f * System.Math.Min (20, levelNumber);
			
			minScore = 1 + levelNumber;
			maxScore = 4 + levelNumber;
			
			speedUpsCount = levelNumber / 5;
			rotatorsCount = levelNumber / 10;
			hidersCount = levelNumber > 3 ? 1 : 0;
			
			maxTimeBonus = 2;
			minTimeBonus = 1;
			bonusTime = 5;
		}
	}
}
