using System.Collections.Generic;
using System;
using Model.Data;

namespace Model.Decorators
{
	class SpeedUpChain: IComparable
	{
		public List<NodeVO> nodes = new List<NodeVO> ();
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
			if (LevelModel.Instance().speedUpsCount == 0)
				return;
				
			List<SpeedUpChain> chains = new List<SpeedUpChain> ();
				
			foreach (NodeVO deadEnd in mazeData.deadEnds) {
				
				NodeVO node = deadEnd;
				int currentDirection = -1;
				SpeedUpChain currentChain = new SpeedUpChain ();
				
				do {
					NodeVO previousNode = node.previousNode;
			
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
							NodeVO.SPECIALS_ROTATOR_CW |
							NodeVO.SPECIALS_ROTATOR_CCW |
							NodeVO.SPECIALS_HIDE_WALLS |
							NodeVO.SPECIALS_SHOW_WALLS); 
					
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
				if (i >= LevelModel.Instance().speedUpsCount) 
					break;
			
				//mark nodes to contain according speedup flags
				foreach (NodeVO nodeData in chains[i].nodes) {
					switch (chains [i].direction) {
					case (NodeVO.DIRECTION_UP_IDX):
						nodeData.AddFlag (NodeVO.SPECIALS_SPEEDUP_UP);
						break;
						
					case (NodeVO.DIRECTION_RIGHT_IDX):
						nodeData.AddFlag (NodeVO.SPECIALS_SPEEDUP_RIGHT);
						break;
						
					case (NodeVO.DIRECTION_DOWN_IDX):
						nodeData.AddFlag (NodeVO.SPECIALS_SPEEDUP_DOWN);
						break;
						
					case (NodeVO.DIRECTION_LEFT_IDX):
						nodeData.AddFlag (NodeVO.SPECIALS_SPEEDUP_LEFT);
						break;
					}
				}
			}
		}
	}
}
