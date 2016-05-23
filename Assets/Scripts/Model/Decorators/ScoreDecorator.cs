using Model.Data;

namespace Model.Decorators
{
	/**
	 * Populates maze nodes with score values according to maze configuration
	 */
	public class ScoreDecorator
	{
		public static void Apply (MazeModel mazeData)
		{
			foreach (NodeVO deadEnd in mazeData.deadEnds) {
				
				NodeVO node = deadEnd;
				int currentScore = DifficultyModel.Instance().minScore;
				int ds = 1;
				
				while (node!=null && node.score == 0) {
				
					currentScore += ds;
					node.score = currentScore;
					//define score delta
					if (node.score <= DifficultyModel.Instance().minScore)
						ds = 1;
					
					if (node.score >= DifficultyModel.Instance().maxScore)
						ds = -1;
					
					node = node.previousNode;
				}
			}
		}
	}
}