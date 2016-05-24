
using System.Collections.Generic;
using System;
using Model;
using Model.Data;

namespace Model.Decorators
{
	public class RotatorDecorator
	{
		public static void Apply (MazeModel mazeData)
		{
			if (DifficultyModel.Instance().rotatorsCount == 0)
				return;

			List<NodeVO> candidates = new List<NodeVO> ();

			foreach (NodeVO node in mazeData.crossRoads) {
			
				//make sure there are no special flags yet
				if (node.HasFlag (
					NodeVO.SPECIALS_SPEEDUP_UP |
					NodeVO.SPECIALS_SPEEDUP_RIGHT |
					NodeVO.SPECIALS_SPEEDUP_DOWN |
					NodeVO.SPECIALS_SPEEDUP_LEFT |
					NodeVO.SPECIALS_HIDE_WALLS |
					NodeVO.SPECIALS_SHOW_WALLS))
					continue;
					
				//make sure its not the start node
				if (node.previousNode == null)
					continue;

				candidates.Add (node);
			}

			Shuffle (candidates);
			for (int i =0; i < candidates.Count; i++) {
				if (i >= DifficultyModel.Instance().rotatorsCount) 
					break;
				
				uint type = 0;
				
				//entering direction to this node
				int prevDirection = candidates [i].previousNode.GetDirectionTowards (candidates [i]);
				
				//calculate the delta direction towards exit
				int delta = candidates [i].directionToExit - prevDirection;
				if (delta > 2)
					delta -= 4;
				if (delta < -2)
					delta += 4;

				if (delta == -1)
					type = NodeVO.SPECIALS_ROTATOR_CCW;
				else if (delta == 1)
					type = NodeVO.SPECIALS_ROTATOR_CW;
				
				candidates [i].AddFlag (type);
			}
		}

		private static void Shuffle<T> (List<T> list)
		{
			int n = list.Count;
			Random rnd = new Random ();
			while (n > 1) {
				int k = (rnd.Next (0, n) % n);
				n--;
				T value = list [k];
				list [k] = list [n];
				list [n] = value;
			}
		}
	}
}