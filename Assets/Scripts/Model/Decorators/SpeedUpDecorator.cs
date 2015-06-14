using System.Collections.Generic;
using System;
using UnityEngine;

namespace AssemblyCSharp
{
	class SpeedUpChain: IComparable
	{
		public List<NodeData> nodes = new List<NodeData> ();
		public int direction;
		
		int IComparable.CompareTo (object other)
		{
			return ((SpeedUpChain)other).nodes.Count - nodes.Count;
		}
	}

	/**
	 * Populates maze nodes with speedup flags according to maze configuration
	 */
	public class SpeedUpDecorator
	{
		public static void Apply (MazeData mazeData)
		{
			if (mazeData.config.speedUpsCount == 0)
				return;
				
			List<SpeedUpChain> chains = new List<SpeedUpChain> ();
				
			foreach (NodeData deadEnd in mazeData.deadEnds) {
				
				NodeData node = deadEnd;
				int currentDirection = -1;
				SpeedUpChain currentChain = new SpeedUpChain ();
				
				do {
					NodeData previousNode = node.previousNode;
			
					bool nodeHasSpeedUp = false;
					foreach (SpeedUpChain chain in chains) {
						if (chain.nodes.Contains (node)) {
							nodeHasSpeedUp = true;
							break;
						}
					}
					
					if (nodeHasSpeedUp) //branch reached a point of another branch that has already been processed
						break;
									
					if (previousNode != null) {
					
						currentChain.nodes.Add (node);
					
						bool hasSpecial = previousNode.HasFlag (
							NodeData.SPECIALS_ROTATOR_CW |
							NodeData.SPECIALS_ROTATOR_CCW |
							NodeData.SPECIALS_HIDE_WALLS |
							NodeData.SPECIALS_SHOW_WALLS); 
					
						int direction = previousNode.GetDirectionTowards (node);
						
						if ((currentDirection != direction) || hasSpecial) {
							if (currentChain != null && currentChain.nodes.Count > 1)
								chains.Add (currentChain);
								
							currentChain = new SpeedUpChain ();
							currentChain.direction = direction;
						}			
							
						currentDirection = hasSpecial ? -1 : direction;
					}
					node = previousNode;
				} while (node!=null);
			}
			
			chains.Sort ();
			
			for (int i =0; i < chains.Count; i++) {
				if (i >= mazeData.config.speedUpsCount) 
					break;
			
				//mark nodes to contain according speedup flags
				foreach (NodeData nodeData in chains[i].nodes) {
					switch (chains [i].direction) {
					case (NodeData.DIRECTION_UP_IDX):
						nodeData.AddFlag (NodeData.SPECIALS_SPEEDUP_UP);
						break;
						
					case (NodeData.DIRECTION_RIGHT_IDX):
						nodeData.AddFlag (NodeData.SPECIALS_SPEEDUP_RIGHT);
						break;
						
					case (NodeData.DIRECTION_DOWN_IDX):
						nodeData.AddFlag (NodeData.SPECIALS_SPEEDUP_DOWN);
						break;
						
					case (NodeData.DIRECTION_LEFT_IDX):
						nodeData.AddFlag (NodeData.SPECIALS_SPEEDUP_LEFT);
						break;
					}
				}
			}
		}
	}
}
