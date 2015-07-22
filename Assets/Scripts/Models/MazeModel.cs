
using System;
using System.Collections;
using System.Collections.Generic;
using Models.Data;

namespace Models
{
	///<summary>
	/// Incapsulates an array of NodeData and its generation. Provides access by x, y coords. 
	///</summary>
	public class MazeModel
	{
		/**
		 * Array of NodeData that represent dead ends generated during maze creation.
		 * Dead ends cant be any others node previos. Sorted by descending distance from starting point
		 */
		public List<NodeModel> deadEnds = new List<NodeModel> ();

		/**
		 * Array of NodeData that represent nodes with more then 3 exits.
		 */
		public List<NodeModel> crossRoads = new List<NodeModel> ();

		/**
		 * currect maze config
		 */
		public MazeConfig config { get { return _config; } }

		private MazeConfig _config;

		//incapsulated data array
		private NodeModel[] _data;
		private System.Random _rnd = new System.Random ();

		public MazeModel (MazeConfig config, int startX, int startY)
		{			
			_config = config;
			
			_data = new NodeModel[_config.width * _config.height];
			
			for (int j = 0; j < _config.width; j++)
				for (int i = 0; i < _config.height; i++)
					_data [i + j * _config.height] = new NodeModel (i, j);
								
			//1. get starting point
			NodeModel startingNode = GetNode (startX, startY);
			startingNode.AddFlag (NodeModel.PROCESSED);
			
			//2. init edge nodes from its neighbours
			List<NodeModel> edgeNodes = GetNotProcessedNeighboursOf (startingNode);
			
			foreach (NodeModel nodeData in edgeNodes) {
				nodeData.previousNode = startingNode;
			}
			
			//3. create branches from edge nodes
			while (edgeNodes.Count>0) {
				
				//3.1 find a random edge node and remove it from array
				int idx = _rnd.Next (0, edgeNodes.Count);
				NodeModel edgeNode = edgeNodes [idx];
				edgeNodes.RemoveAt (idx);
				
				if (!edgeNode.HasFlag (NodeModel.PROCESSED)) {
					
					//3.2 attach it to current tree
					NodeModel processedNeighbour = GetRandomNeighbour (edgeNode, true);
					if (processedNeighbour != null) {
						Merge (processedNeighbour, edgeNode);
						edgeNode.previousNode = processedNeighbour;
						
						if (!crossRoads.Contains (processedNeighbour)) 
							crossRoads.Add (processedNeighbour);
					} 
					
					//3.3 create the branch
					CreateBranch (edgeNode, edgeNodes);
				}
			}
		}

		/**
		 * Gets the NodeData at provided 2d coordinates 
		 */
		public NodeModel GetNode (int x, int y)
		{
			return _data [x + y * _config.width];
		}
		
		private void CreateBranch (NodeModel startNode, List<NodeModel> edgeNodes)
		{
			NodeModel randomNeighbour;
			NodeModel currentNode = startNode;
			
			do {
				//1. if node exists in edge nodes, remove it
				if (edgeNodes.Contains (currentNode))
					edgeNodes.Remove (currentNode);
				
				//1.1 append new edge nodes
				List<NodeModel> notProcessedNeighbours = GetNotProcessedNeighboursOf (currentNode);
				foreach (NodeModel nodeData in notProcessedNeighbours) {
					if (!edgeNodes.Contains (nodeData)) {
						edgeNodes.Add (nodeData);
					}
				}
				
				currentNode.AddFlag (NodeModel.PROCESSED);
				
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
		private List<NodeModel> GetNotProcessedNeighboursOf (NodeModel target)
		{
			List<NodeModel> neighbours = new List<NodeModel> ();
			
			for (int i = 0; i < 4; i++) {
				
				int x = target.pos.x + NodeModel.DIRECTIONS [i, 0];
				int y = target.pos.y + NodeModel.DIRECTIONS [i, 1];
				
				if (IsInBounds (x, y)) {
					NodeModel neighbour = GetNode (x, y);
					if (!neighbour.HasFlag (NodeModel.PROCESSED))
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
		private NodeModel GetRandomNeighbour (NodeModel target, bool processedNeeded)
		{
			int offset = _rnd.Next (0, 4);
			for (int i = 0; i < 4; i++) {
				int dir = (offset + i) % 4;
				
				int x = target.pos.x + NodeModel.DIRECTIONS [dir, 0];
				int y = target.pos.y + NodeModel.DIRECTIONS [dir, 1];
				
				if (IsInBounds (x, y)) {
					NodeModel neighbour = GetNode (x, y);
					if ((neighbour.HasFlag (NodeModel.PROCESSED) && processedNeeded) || (!neighbour.HasFlag (NodeModel.PROCESSED) && !processedNeeded))
						return neighbour;
				}
			}
			
			return null;
		}
		
		/**
		 * Removes walls between two specified nodes
		 */
		private void Merge (NodeModel from, NodeModel to)
		{
			int dx = to.pos.x - from.pos.x;
			int dy = to.pos.y - from.pos.y;
			
			if (dx != 0) {
				if (dx > 0) {
					to.RemoveWall (NodeModel.DIRECTION_LEFT_IDX);
					from.RemoveWall (NodeModel.DIRECTION_RIGHT_IDX);
				} else {
					to.RemoveWall (NodeModel.DIRECTION_RIGHT_IDX);
					from.RemoveWall (NodeModel.DIRECTION_LEFT_IDX);
				}
			} else if (dy != 0) {
				if (dy > 0) {
					to.RemoveWall (NodeModel.DIRECTION_DOWN_IDX);
					from.RemoveWall (NodeModel.DIRECTION_UP_IDX);
				} else {
					to.RemoveWall (NodeModel.DIRECTION_UP_IDX);
					from.RemoveWall (NodeModel.DIRECTION_DOWN_IDX);
				}
			}
		}
	}
}

