using System;
using Models;

namespace Models.Decorators
{
	/**
	 * Populates maze nodes with hide/unhide flags according to maze configuration
	 */
	public class HiderDecorator
	{
		private static System.Random _rnd = new System.Random ();

		public static void Apply (MazeModel mazeData)
		{
			if (mazeData.config.hidersCount == 0)
				return;

			for (int i =0; i < mazeData.deadEnds.Count; i++) {
				if (i >= mazeData.config.hidersCount)
					break;

				int distance = (int)mazeData.deadEnds [i].GetDistance ();
				
				if (distance < 4)
					return;
				
				int showIndex = _rnd.Next ((int)distance / 4, (int)distance / 2);
				int hideIndex = _rnd.Next (showIndex + 2, showIndex + (int)Math.Min((int)(distance / 2), 8));
				
				NodeModel node = mazeData.deadEnds [i].previousNode;
				int index = 0;
				while (node !=null) {
					index++;

					if (index == showIndex)
						node.AddFlag (NodeModel.SPECIALS_SHOW_WALLS);

					
					if (index == hideIndex)
						node.AddFlag (NodeModel.SPECIALS_HIDE_WALLS);

					node = node.previousNode;
				}
			}
		}
	}
}
