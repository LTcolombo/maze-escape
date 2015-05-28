
using System.Collections.Generic;
using System;

namespace AssemblyCSharp
{
	public class RotatorDecorator
	{

		public static void Apply (MazeData mazeData)
		{
			List<NodeData> candidates = new List<NodeData> ();

			foreach (NodeData deadEnd in mazeData.deadEnds) {
				
				NodeData node = deadEnd;
				int currentDirection = -1;

				while (node!=null && !candidates.Contains(node)) {
					NodeData nextNode = node.previousNode;
					
					if (!node.HasFlag(
						NodeData.SPECIALS_SPEEDUP_UP |
						NodeData.SPECIALS_SPEEDUP_RIGHT |
						NodeData.SPECIALS_SPEEDUP_DOWN |
						NodeData.SPECIALS_SPEEDUP_LEFT)){

						if (nextNode != null) {
							int direction = GetDirection (nextNode, node);

							if (currentDirection>-1 && currentDirection != direction) {							
								candidates.Add (node);
							}				
							currentDirection = direction;
						}
					}
					node = nextNode;
				}
			}

			Shuffle (candidates);
			for (int i =0; i < candidates.Count; i++) {
				if (i >= mazeData.config.rotatorsCount) 
					break;
				candidates[i].AddFlag (NodeData.SPECIALS_ROTATOR_CW);
			}
		}

		static int GetDirection (NodeData nextNode, NodeData node)
		{
			//presumably nodes are next to each other
			//inverse direction to make speedup towards dead ends (and exit)
			if (nextNode.x == node.x) {
				if (nextNode.y < node.y)
					return NodeData.DIRECTION_UP_IDX;
				else
					return NodeData.DIRECTION_DOWN_IDX;
			} else {
				if (nextNode.x < node.x)
					return NodeData.DIRECTION_RIGHT_IDX;
				else
					return NodeData.DIRECTION_LEFT_IDX;
			}
		}

		public static void Shuffle(List<NodeData> list) {
			int n = list.Count;
			Random rnd = new Random();
			while (n > 1) {
				int k = (rnd.Next(0, n) % n);
				n--;
				NodeData value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}