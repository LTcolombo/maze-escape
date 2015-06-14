
namespace AssemblyCSharp
{
	public class ExitDecorator
	{

		public static void Apply (MazeData mazeData)
		{
			if (mazeData.deadEnds.Count > 0)
				mazeData.deadEnds [0].AddFlag (NodeData.SPECIALS_EXIT);
		}
	}
}

