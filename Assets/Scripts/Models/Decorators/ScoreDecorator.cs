using Models;

namespace Models.Decorators
{
	/**
	 * Populates maze nodes with score values according to maze configuration
	 */
	public class ScoreDecorator
	{
		public static void Apply (MazeModel mazeData)
		{
			foreach (NodeModel deadEnd in mazeData.deadEnds) {
				
				NodeModel node = deadEnd;
				int currentScore = mazeData.config.minScore;
				int ds = 1;
				
				while (node!=null && node.score == 0) {
				
					currentScore += ds;
					node.score = currentScore;
					//define score delta
					if (node.score <= mazeData.config.minScore)
						ds = 1;
					
					if (node.score >= mazeData.config.maxScore)
						ds = -1;
					
					node = node.previousNode;
				}
			}
		}
	}
}