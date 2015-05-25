using System.Collections;
using System.Collections.Generic;
using System;

namespace AssemblyCSharp
{
	public class MazeGenerator
	{
		private static MazeData _maze;
		private static List<NodeData> _edgeNodes;
		private static List<NodeData> _deadEnds;
		private static NodeData _lastNode;
		private static System.Random _rnd;
		
		/**
     * Creates a maze.
     */
		public static MazeData Generate (int width, int height, int startX, int startY)
		{
			if (_rnd == null)
				_rnd = new System.Random ();
				
			_deadEnds = new List<NodeData> ();
			
			_maze = new MazeData (width, height);

			//1. get starting point
			NodeData startingNode = _maze.GetNode (startX, startY);
			startingNode.AddFlag (NodeData.PROCESSED);
						
			_lastNode = startingNode;
			
			//2. init edge nodes from its neighbours
			_edgeNodes = _maze.GetNotProcessedNeighboursOf (startingNode);
						
			foreach (NodeData nodeData in _edgeNodes) {
				nodeData.previousNode = startingNode;
			}
			
			//3. create branches from edge nodes
			while (_edgeNodes.Count>0) {
				
				//3.1 find a random edge node and remove it from array
				int idx = _rnd.Next (0, _edgeNodes.Count);
				NodeData edgeNode = _edgeNodes [idx];
				_edgeNodes.RemoveAt (idx);
				
				if (!edgeNode.HasFlag (NodeData.PROCESSED)) {
					
					//3.2 attach it to current tree
					NodeData processedNeighbour = _maze.GetRandomNeighbour (edgeNode, true);
					if (processedNeighbour != null)
						_maze.Merge (processedNeighbour, edgeNode);
					
					//3.3 create the branch
					CreateBranch (edgeNode);
				}
			}
						
			for (int i = 0; i < 5; i++) {
				NodeData nodeData = _maze.GetRandomNode ();
				nodeData.AddFlag (NodeData.SPECIALS_ROTATOR_CW);
			}
						
			for (int i = 0; i < 5; i++) {
				NodeData nodeData = _maze.GetNode (3, i + 1);
				nodeData.AddFlag (NodeData.SPECIALS_SPEEDUP_UP);
			}
						
			for (int i = 0; i < 5; i++) {
				
			}
						
						
			_lastNode.AddFlag (NodeData.SPECIALS_EXIT);

			_maze.movesQuota = _lastNode.GetDistance () * 2;
			return _maze;
		}
		
		private static void CreateBranch (NodeData startNode)
		{
			NodeData randomNeighbour;
			NodeData currentNode = startNode;
			currentNode.score = 0;
			int ds = 0;
			
			do {
				//1. if node exists in edge nodes, remove it
				int idx = _edgeNodes.IndexOf (currentNode);
				if (idx >= 0)
					_edgeNodes.RemoveAt (idx);
				
				currentNode.AddFlag (NodeData.PROCESSED);
				
				//2. go to random direction and get a neighbour
				randomNeighbour = _maze.GetRandomNeighbour (currentNode, false);
				
				//3. if it exists (didn't got a dead end) - expand the maze
				if (randomNeighbour != null) {
					//define dspeed
					if (currentNode.score <= 1)
						ds = 1;
					
					if (currentNode.score >= MazeData.MAX_SCORE)
						ds = -1;
					
					randomNeighbour.score = (currentNode.score + ds);
					randomNeighbour.previousNode = currentNode;
												
					//3.1 attach it to tree
					_maze.Merge (currentNode, randomNeighbour);
										
					//3.2 append new edge nodes
					_edgeNodes.AddRange (_maze.GetNotProcessedNeighboursOf (randomNeighbour));
					
					//3.3 process it on next loop entry
					currentNode = randomNeighbour;
				} else {
				
					_deadEnds.Add (currentNode);
					
					if (_lastNode == null || currentNode.GetDistance () > _lastNode.GetDistance ())
						_lastNode = currentNode;
				}
				
			} while (randomNeighbour!=null);
		}
	}
}