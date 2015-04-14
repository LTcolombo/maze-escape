using System.Collections;
using System.Collections.Generic;
using System;

namespace AssemblyCSharp
{
		public class MazeGenerator
		{
				private static MazeData _maze;
				private static List<NodeData> _edgeNodes;
				private static System.Random _rnd;
		
				/**
     * Creates a maze.
     */
				public static MazeData Generate (int width, int height)
				{
						if (_rnd == null)
								_rnd = new System.Random ();
						_maze = new MazeData (width, height);

						//1. get starting point
						NodeData startingNode = _maze.GetRandomNode ();
						startingNode.processed = true;
			
						//2. init edge nodes from its neighbours
						_edgeNodes = _maze.GetNotProcessedNeighboursOf (startingNode);
			
						//3. create branches from edge nodes
						while (_edgeNodes.Count>0) {
				
								//3.1 find a random edge node and remove it from array
								int idx = _rnd.Next (0, _edgeNodes.Count);
								NodeData edgeNode = _edgeNodes [idx];
								_edgeNodes.RemoveAt (idx);
				
								if (!edgeNode.processed) {
					
										//3.2 attach it to current tree
										NodeData processedNeighbour = _maze.GetRandomNeighbour (edgeNode, true);
										if (processedNeighbour != null)
												_maze.Merge (processedNeighbour, edgeNode);
					
										//3.3 create the branch
										CreateBranch (edgeNode);
								}
						}

						return _maze;
				}
		
				private static void CreateBranch (NodeData startNode)
				{
						NodeData randomNeighbour;
						NodeData currentNode = startNode;
						currentNode.speed = 0;
						int dspeed = 0;
			
						do {
								//1. if node exists in edge nodes, remove it
								int idx = _edgeNodes.IndexOf (currentNode);
								if (idx >= 0)
										_edgeNodes.RemoveAt (idx);
				
								currentNode.processed = true;
				
								//2. go to random direction and get a neighbour
								randomNeighbour = _maze.GetRandomNeighbour (currentNode, false);
				
								//3. if it exists (didn't got a dead end) - expand the maze
								if (randomNeighbour != null) {
										//define dspeed
										if (currentNode.speed < 1)
												dspeed = 1;
					
										if (currentNode.speed > 4)
												dspeed = -1;
					
										randomNeighbour.speed = currentNode.speed += dspeed;
					
										//3.1 attach it to tree, append new edge nodes
										_maze.Merge (currentNode, randomNeighbour);
										_edgeNodes.AddRange (_maze.GetNotProcessedNeighboursOf (randomNeighbour));
					
										//3.2 process it on next loop entry
										currentNode = randomNeighbour;
								} else {
										//3.1 merge to a random cell
										_maze.Merge (currentNode, _maze.GetRandomUnmergedNeighbour (currentNode));
								}
				
						} while (randomNeighbour!=null);
				}
		}
}