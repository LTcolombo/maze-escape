using Models;

namespace Models.Decorators
{
	public class ExitDecorator
	{

		public static void Apply (MazeModel mazeData)
		{
			if (mazeData.deadEnds.Count > 0)
				mazeData.deadEnds [0].AddFlag (NodeModel.SPECIALS_EXIT);
		}
	}
}

