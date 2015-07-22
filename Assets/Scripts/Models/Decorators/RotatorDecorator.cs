
using System.Collections.Generic;
using System;
using Models;

namespace Models.Decorators
{
	class RotatorCandidate
	{
		public NodeModel node;
		public int exitDirection;
		
		public RotatorCandidate (NodeModel node, int exitDirection)
		{
			this.node = node;
			this.exitDirection = exitDirection;
		}
	}

	public class RotatorDecorator
	{
		public static void Apply (MazeModel mazeData)
		{
			if (mazeData.config.rotatorsCount == 0)
				return;
			
			List<NodeModel> exitChain = new List<NodeModel> ();
			NodeModel exitChainNode = mazeData.deadEnds [0];
			while (exitChainNode!=null) {
				exitChain.Add (exitChainNode);
				exitChainNode = exitChainNode.previousNode;
			}
				
			List<RotatorCandidate> candidates = new List<RotatorCandidate> ();

			foreach (NodeModel node in mazeData.crossRoads) {
			
				//make sure there are no special flags yet
				if (node.HasFlag (
					NodeModel.SPECIALS_SPEEDUP_UP |
					NodeModel.SPECIALS_SPEEDUP_RIGHT |
					NodeModel.SPECIALS_SPEEDUP_DOWN |
					NodeModel.SPECIALS_SPEEDUP_LEFT |
					NodeModel.SPECIALS_HIDE_WALLS |
					NodeModel.SPECIALS_SHOW_WALLS))
					continue;
					
				//make sure its not the start node
				if (node.previousNode == null)
					continue;
					
				//if this node is within path to exit - mark the direction towards exit
				int indexInExitChain = exitChain.IndexOf (node);
				if (indexInExitChain > 0)
					candidates.Add (new RotatorCandidate (node, node.GetDirectionTowards (exitChain [indexInExitChain - 1])));
				else 
					candidates.Add (new RotatorCandidate (node, -1));
			}

			Shuffle (candidates);
			for (int i =0; i < candidates.Count; i++) {
				if (i >= mazeData.config.rotatorsCount) 
					break;
				
				uint type = 0;
				
				//entering direction to this node
				int prevDirection = candidates [i].node.previousNode.GetDirectionTowards (candidates [i].node);
				
				//if this node is within path to exit
				if (candidates [i].exitDirection > -1) {
					
					//calculate the delta direction towards exit
					int delta = candidates [i].exitDirection - prevDirection;
					if (delta > 2)
						delta -= 4;
					if (delta < -2)
						delta += 4;
						
					if (delta == -1)
						type = NodeModel.SPECIALS_ROTATOR_CCW;
					else if (delta == 1)
						type = NodeModel.SPECIALS_ROTATOR_CW;
					
				} else {
					//try to make a rotation not in the wall
					int randomTurnIndex = (i % 2 == 0) ? 1 : -1;
					if (!candidates [i].node.HasWall (NormaliseDirection (prevDirection + randomTurnIndex)))
						type = (randomTurnIndex == 1) ? NodeModel.SPECIALS_ROTATOR_CW : NodeModel.SPECIALS_ROTATOR_CCW;
					else if (!candidates [i].node.HasWall (NormaliseDirection (prevDirection - randomTurnIndex)))
						type = (randomTurnIndex == 1) ? NodeModel.SPECIALS_ROTATOR_CCW : NodeModel.SPECIALS_ROTATOR_CW;
					else 
						type = i % 2 == 0 ? NodeModel.SPECIALS_ROTATOR_CW : NodeModel.SPECIALS_ROTATOR_CCW;
				}
				
				if (type > 0)
					candidates [i].node.AddFlag (type);
			}
		}

		
		/**
		* normalises direction index to [0..3] values
		*/
		private static int NormaliseDirection (int direction_idx)
		{
			while (direction_idx < 0)
				direction_idx += 4;
			
			while (direction_idx > 3)
				direction_idx -= 4;
			
			return direction_idx;
		}

		private static void Shuffle (List<RotatorCandidate> list)
		{
			int n = list.Count;
			Random rnd = new Random ();
			while (n > 1) {
				int k = (rnd.Next (0, n) % n);
				n--;
				RotatorCandidate value = list [k];
				list [k] = list [n];
				list [n] = value;
			}
		}
	}
}