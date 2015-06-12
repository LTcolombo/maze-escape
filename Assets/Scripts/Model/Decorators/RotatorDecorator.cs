
using System.Collections.Generic;
using System;

namespace AssemblyCSharp
{
	public class RotatorDecorator
	{

		public static void Apply (MazeData mazeData)
		{

			if (mazeData.config.rotatorsCount == 0)
				return;

			List<NodeData> candidates = new List<NodeData> ();

			foreach (NodeData deadEnd in mazeData.deadEnds) {
				
				NodeData node = deadEnd;
				int currentDirection = -1;

				while (node!=null && !candidates.Contains(node)) {
					NodeData previousNode = node.previousNode;
					
					if (!node.HasFlag (
						NodeData.SPECIALS_SPEEDUP_UP |
						NodeData.SPECIALS_SPEEDUP_RIGHT |
						NodeData.SPECIALS_SPEEDUP_DOWN |
						NodeData.SPECIALS_SPEEDUP_LEFT)) {

						if (previousNode != null) {
							int direction = previousNode.GetDirectionTowards (node);

							if (currentDirection > -1 && currentDirection != direction) {							
								candidates.Add (node);
							}				
							currentDirection = direction;
						}
					}
					node = previousNode;
				}
			}

			Shuffle (candidates);
			for (int i =0; i < candidates.Count; i++) {
				if (i >= mazeData.config.rotatorsCount) 
					break;
				candidates [i].AddFlag (i % 2 == 0 ? NodeData.SPECIALS_ROTATOR_CW : NodeData.SPECIALS_ROTATOR_CCW);
			}
		}

		public static void Shuffle (List<NodeData> list)
		{
			int n = list.Count;
			Random rnd = new Random ();
			while (n > 1) {
				int k = (rnd.Next (0, n) % n);
				n--;
				NodeData value = list [k];
				list [k] = list [n];
				list [n] = value;
			}
		}
	}
}