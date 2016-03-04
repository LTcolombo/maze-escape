
using System;

namespace Model
{
	///<summary>
	/// A structure that specifies the params of level
	///</summary>
	public class LevelModel
	{
		public static int NODE_SIZE = 128;

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
		public float scoreDrainTime;
		public bool isTutorial;

		private static LevelModel _instance;

		public static LevelModel Instance ()
		{
			if (_instance == null)
				_instance = new LevelModel ();

			return _instance;
		}

		private LevelModel (){}

		public void SetNumber(int levelNumber){
		{			
			int size = 3 + levelNumber / 4;
			
			if (size > 10)
				size = 10;
			
			width = size;
			height = size;
			
			moveTime = 1;//0.4f - 0.01f * System.Math.Min (15, levelNumber) - 0.005f * Math.Max (0, Math.Min (10, levelNumber - 15));
			
			minScore = 1 + size;
			maxScore = 4 + size;
			
			speedUpsCount = levelNumber / 6;
			if (speedUpsCount > 4)
				speedUpsCount = 4;
				
			rotatorsCount = levelNumber > 3 ? 1 : 0;

			rotatorsCount += levelNumber / 10;
			
			hidersCount = levelNumber > 2 ? 1 : 0;
			
			maxTimeBonus = 1 + size / 10.0f;
			minTimeBonus = 1;
			bonusTime = size / 2.0f;
			
			scoreDrainTime = 1.0f;
			
			isTutorial = true;// levelNumber <= 3;
			}	
	}
	}
}
