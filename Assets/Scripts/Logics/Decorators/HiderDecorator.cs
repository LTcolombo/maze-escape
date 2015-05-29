namespace AssemblyCSharp
{
	public class HiderDecorator
	{
		private static System.Random _rnd = new System.Random ();

		public static void Apply (MazeData mazeData)
		{
			if (mazeData.config.hidersCount == 0)
				return;

			for (int i =0; i < mazeData.deadEnds.Count; i++) {
				if (i > mazeData.config.hidersCount)
					break;

				int distance = (int)mazeData.deadEnds [i].GetDistance ();

				const int OFFSET = 2;

				//todo
				int turnOff = _rnd.Next (OFFSET, (int)distance / 2);
				int turnOn = _rnd.Next (turnOff + 2, turnOff + 6);

				if (turnOff > distance - OFFSET)
					turnOff = distance - OFFSET;

				NodeData node = mazeData.deadEnds [i].previousNode;
				int index = 0;
				while (node !=null) {
					index++;

					if (index == turnOff)
						node.AddFlag (NodeData.SPECIALS_SHOW_WALLS);

					
					if (index == turnOn)
						node.AddFlag (NodeData.SPECIALS_HIDE_WALLS);

					node = mazeData.deadEnds [i].previousNode;
				}
			}
		}
	}
}
