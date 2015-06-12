
using System;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	///<summary>
	/// Incapsulates an array of NodeData and its generation. Provides access by x, y coords. 
	///</summary>
	public class MazeData
	{
		/**
		 * Array of NodeData that represent dead ends generated during maze creation.
		 * Dead ends cant be any others node previos. Sorted by descending distance from starting point
		 */
		public List<NodeData> deadEnds = new List<NodeData> ();

		/**
		 * currect maze config
		 */
		public MazeConfig config { get { return _config; } }

		private MazeConfig _config;

		//incapsulated data array
		private NodeData[] _data;
		private System.Random _rnd = new System.Random ();

		public MazeData (MazeConfig config, int startX, int startY)
		{			
			_config = config;
			
			_data = new NodeData[_config.width * _config.height];
			
			for (int j = 0; j < _config.width; j++)
				for (int i = 0; i < _config.height; i++)
					_data [i + j * _config.height] = new NodeData (i, j);
								
			//1. get starting point
			NodeData startingNode = GetNode (startX, startY);
			startingNode.AddFlag (NodeData.PROCESSED);
			
			//2. init edge nodes from its neighbours
			List<NodeData> edgeNodes = GetNotProcessedNeighboursOf (startingNode);
			
			foreach (NodeData nodeData in edgeNodes) {
				nodeData.previousNode = startingNode;
			}
			
			//3. create branches from edge nodes
			while (edgeNodes.Count>0) {
				
				//3.1 find a random edge node and remove it from array
				int idx = _rnd.Next (0, edgeNodes.Count);
				NodeData edgeNode = edgeNodes [idx];
				edgeNodes.RemoveAt (idx);
				
				if (!edgeNode.HasFlag (NodeData.PROCESSED)) {
					
					//3.2 attach it to current tree
					NodeData processedNeighbour = GetRandomNeighbour (edgeNode, true);
					if (processedNeighbour != null) {
						Merge (processedNeighbour, edgeNode);
						edgeNode.previousNode = processedNeighbour;
					} 
					
					//3.3 create the branch
					CreateBranch (edgeNode, edgeNodes);
				}
			}

			if (deadEnds.Count > 0)
				deadEnds [0].AddFlag (NodeData.SPECIALS_EXIT);
		}

		/**
		 * Gets the NodeData at provided 2d coordinates 
		 */
		public NodeData GetNode (int x, int y)
		{
			return _data [x + y * _config.width];
		}
		
		private void CreateBranch (NodeData startNode, List<NodeData> edgeNodes)
		{
			NodeData randomNeighbour;
			NodeData currentNode = startNode;
			
			do {
				//1. if node exists in edge nodes, remove it
				if (edgeNodes.Contains (currentNode))
					edgeNodes.Remove (currentNode);
				
				//1.1 append new edge nodes
				List<NodeData> notProcessedNeighbours = GetNotProcessedNeighboursOf (currentNode);
				foreach (NodeData nodeData in notProcessedNeighbours) {
					if (!edgeNodes.Contains (nodeData)) {
						edgeNodes.Add (nodeData);
					}
				}
				
				currentNode.AddFlag (NodeData.PROCESSED);
				
				//2. go to random direction and get a neighbour
				randomNeighbour = GetRandomNeighbour (currentNode, false);
				
				//3. if it exists (didn't got a dead end) - expand the maze
				if (randomNeighbour != null) {
					randomNeighbour.previousNode = currentNode;
					
					//3.1 attach it to tree
					Merge (currentNode, randomNeighbour);
					
					//3.2 process it on next loop entry
					currentNode = randomNeighbour;
				} else {
					
					if (deadEnds.Count > 0 && (currentNode.GetDistance () > deadEnds [0].GetDistance ())) {
						deadEnds.Insert (0, currentNode);
					} else
						deadEnds.Add (currentNode);
				}
				
			} while (randomNeighbour!=null);
		}
		
		/**
         * Gets all neighbours of specified node not processed by alghoritm.
         */
		private List<NodeData> GetNotProcessedNeighboursOf (NodeData target)
		{
			List<NodeData> neighbours = new List<NodeData> ();
			
			for (int i = 0; i < 4; i++) {
				
				int x = target.x + NodeData.DIRECTIONS [i, 0];
				int y = target.y + NodeData.DIRECTIONS [i, 1];
				
				if (IsInBounds (x, y)) {
					NodeData neighbour = GetNode (x, y);
					if (!neighbour.HasFlag (NodeData.PROCESSED))
						neighbours.Add (neighbour);
				}
			}
			return neighbours;
		}
		
		/**
		 * Checks if provided 2d coordinates are within 1d data array
		 */
		private bool IsInBounds (int x, int y)
		{
			return ((x > -1) && (x < _config.width) && (y > -1) && (y < _config.height));
		}
		
		/**
		 * Finds a random neighbour with specified param
		 */
		private NodeData GetRandomNeighbour (NodeData target, bool processedNeeded)
		{
			int offset = _rnd.Next (0, 4);
			for (int i = 0; i < 4; i++) {
				int dir = (offset + i) % 4;
				
				int x = target.x + NodeData.DIRECTIONS [dir, 0];
				int y = target.y + NodeData.DIRECTIONS [dir, 1];
				
				if (IsInBounds (x, y)) {
					NodeData neighbour = GetNode (x, y);
					if ((neighbour.HasFlag (NodeData.PROCESSED) && processedNeeded) || (!neighbour.HasFlag (NodeData.PROCESSED) && !processedNeeded))
						return neighbour;
				}
			}
			
			return null;
		}
		
		/**
		 * Removes walls between two specified nodes
		 */
		private void Merge (NodeData from, NodeData to)
		{
			int dx = to.x - from.x;
			int dy = to.y - from.y;
			
			if (dx != 0) {
				if (dx > 0) {
					to.RemoveWall (NodeData.DIRECTION_LEFT_IDX);
					from.RemoveWall (NodeData.DIRECTION_RIGHT_IDX);
				} else {
					to.RemoveWall (NodeData.DIRECTION_RIGHT_IDX);
					from.RemoveWall (NodeData.DIRECTION_LEFT_IDX);
				}
			} else if (dy != 0) {
				if (dy > 0) {
					to.RemoveWall (NodeData.DIRECTION_DOWN_IDX);
					from.RemoveWall (NodeData.DIRECTION_UP_IDX);
				} else {
					to.RemoveWall (NodeData.DIRECTION_UP_IDX);
					from.RemoveWall (NodeData.DIRECTION_DOWN_IDX);
				}
			}
		}
	}
}

