using Model;
using Model.Data;

namespace Model.Decorators
{
	public class ExitDecorator
	{

		public static void Apply (MazeModel mazeData)
		{
			if (mazeData.deadEnds.Count > 0)
				mazeData.deadEnds [0].AddFlag (NodeVO.SPECIALS_EXIT);
		}
	}
}

