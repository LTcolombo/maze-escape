using System.Collections.Generic;
using System;
using Models;

namespace Models.Decorators
{
	class SpeedUpChain: IComparable
	{
		public List<NodeModel> nodes = new List<NodeModel> ();
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
		public static void Apply (MazeModel mazeData)
		{
			if (mazeData.config.speedUpsCount == 0)
				return;
				
			List<SpeedUpChain> chains = new List<SpeedUpChain> ();
				
			foreach (NodeModel deadEnd in mazeData.deadEnds) {
				
				NodeModel node = deadEnd;
				int currentDirection = -1;
				SpeedUpChain currentChain = new SpeedUpChain ();
				
				do {
					NodeModel previousNode = node.previousNode;
			
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
							NodeModel.SPECIALS_ROTATOR_CW |
							NodeModel.SPECIALS_ROTATOR_CCW |
							NodeModel.SPECIALS_HIDE_WALLS |
							NodeModel.SPECIALS_SHOW_WALLS); 
					
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
				foreach (NodeModel nodeData in chains[i].nodes) {
					switch (chains [i].direction) {
					case (NodeModel.DIRECTION_UP_IDX):
						nodeData.AddFlag (NodeModel.SPECIALS_SPEEDUP_UP);
						break;
						
					case (NodeModel.DIRECTION_RIGHT_IDX):
						nodeData.AddFlag (NodeModel.SPECIALS_SPEEDUP_RIGHT);
						break;
						
					case (NodeModel.DIRECTION_DOWN_IDX):
						nodeData.AddFlag (NodeModel.SPECIALS_SPEEDUP_DOWN);
						break;
						
					case (NodeModel.DIRECTION_LEFT_IDX):
						nodeData.AddFlag (NodeModel.SPECIALS_SPEEDUP_LEFT);
						break;
					}
				}
			}
		}
	}
}
