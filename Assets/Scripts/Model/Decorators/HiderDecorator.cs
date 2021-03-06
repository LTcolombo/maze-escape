using System;
using Model;
using Model.Data;

namespace Model.Decorators
{
	/**
	 * Populates maze nodes with hide/unhide flags according to maze configuration
	 */
	public class HiderDecorator
	{
		private static System.Random _rnd = new System.Random ();

		public static void Apply (MazeModel mazeData)
		{
			if (DifficultyModel.Instance().hidersCount == 0)
				return;

			for (int i =0; i < mazeData.deadEnds.Count; i++) {
				if (i >= DifficultyModel.Instance().hidersCount)
					break;

				int distance = (int)mazeData.deadEnds [i].GetDistance ();
				
				if (distance < 4)
					return;
				
				int showIndex = _rnd.Next ((int)distance / 4, (int)distance / 2);
				int hideIndex = _rnd.Next (showIndex + 2, showIndex + (int)Math.Min((int)(distance / 2), 8));
				
				NodeVO node = mazeData.deadEnds [i].previousNode;
				int index = 0;
				while (node !=null) {
					index++;

					if (index == showIndex)
						node.AddFlag (NodeVO.SPECIALS_SHOW_WALLS);

					
					if (index == hideIndex)
						node.AddFlag (NodeVO.SPECIALS_HIDE_WALLS);

					node = node.previousNode;
				}
			}
		}
	}
}
