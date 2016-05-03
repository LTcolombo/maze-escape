using Model;
using Model.Data;

namespace Model.Decorators
{
	public class ExitDecorator
	{

		public static void Apply (MazeModel mazeData)
		{
			for (int i = 0; i < mazeData.deadEnds.Count; i++) {
				var deadEnd = mazeData.deadEnds [i];
				var isExit = i == 0;
				if (isExit) {
					deadEnd.AddFlag (NodeVO.SPECIALS_EXIT);
					SetDirectionsTowards (deadEnd);
				} else {
					SetDirectionsFrom (deadEnd);
				}	
			}
		}

		static void SetDirectionsTowards (NodeVO deadEnd)
		{
			var currentNode = deadEnd;
			var previousNode = currentNode.previousNode;

			while (previousNode != null && previousNode.directionToExit == NodeVO.DIRECTION_INVALID_IDX) {
				previousNode.directionToExit = previousNode.GetDirectionTowards (currentNode);
				currentNode = previousNode;
				previousNode = currentNode.previousNode;
			}
		}

		static void SetDirectionsFrom (NodeVO deadEnd)
		{
			var currentNode = deadEnd;
			var previousNode = currentNode.previousNode;

			while (previousNode != null && currentNode.directionToExit == NodeVO.DIRECTION_INVALID_IDX) {
				currentNode.directionToExit = currentNode.GetDirectionTowards (previousNode);
				currentNode = previousNode;
				previousNode = currentNode.previousNode;
			}
		}
	}
}

