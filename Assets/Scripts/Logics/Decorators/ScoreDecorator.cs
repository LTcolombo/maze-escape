
namespace AssemblyCSharp
{
	public class ScoreDecorator
	{
		public static void Apply (MazeData mazeData, int minScore, int maxScore)
		{
			foreach (NodeData deadEnd in mazeData.deadEnds) {
				
				NodeData node = deadEnd;
				int currentScore = minScore;
				int ds = 1;
				
				while (node!=null) {
				
					currentScore += ds;
					node.score = currentScore;
					//define score delta
					if (node.score <= minScore)
						ds = 1;
					
					if (node.score >= maxScore)
						ds = -1;
					
					node = node.previousNode;
				}
			}
		}
	}
}