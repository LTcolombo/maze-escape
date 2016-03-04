
using System;
using System.Collections;
using System.Collections.Generic;
using Model.Data;

namespace Model
{
	///<summary>
	/// Incapsulates an array of NodeData and its generation. Provides access by x, y coords. 
	///</summary>
	public class MazeModel
	{
		public NodeVO startingNode;
	
		/**
		 * Array of NodeData that represent dead ends generated during maze creation.
		 * Dead ends cant be any others node previos. Sorted by descending distance from starting point
		 */
		public List<NodeVO> deadEnds = new List<NodeVO> ();

		/**
		 * Array of NodeData that represent nodes with more then 3 exits.
		 */
		public List<NodeVO> crossRoads = new List<NodeVO> ();

		//incapsulated data array
		private NodeVO[] _data;
		private int _width;
		private int _height;
		private System.Random _rnd = new System.Random ();

		private static MazeModel _instance;

		public static MazeModel Instance ()
		{
			if (_instance == null)
				_instance = new MazeModel ();

			return _instance;
		}


		private MazeModel (){
		}

		public void Recreate(int width, int height, int startX, int startY)
		{
			_width = width;
			_height = height;
			_data = new NodeVO[_width * _height];
			
			for (int j = 0; j < _width; j++)
				for (int i = 0; i < _height; i++)
					_data [i + j * _height] = new NodeVO (i, j);
								
			//1. get starting point
			startingNode = GetNode (startX, startY);
			startingNode.AddFlag (NodeVO.PROCESSED);
			
			//2. init edge nodes from its neighbours
			List<NodeVO> edgeNodes = GetNotProcessedNeighboursOf (startingNode);
			
			foreach (NodeVO nodeData in edgeNodes) {
				Link (startingNode, nodeData);
			}
			
			//3. create branches from edge nodes
			while (edgeNodes.Count>0) {
				
				//3.1 find a random edge node and remove it from array
				int idx = _rnd.Next (0, edgeNodes.Count);
				NodeVO edgeNode = edgeNodes [idx];
				edgeNodes.RemoveAt (idx);
				
				if (!edgeNode.HasFlag (NodeVO.PROCESSED)) {
					
					//3.2 attach it to current tree
					NodeVO processedNeighbour = GetRandomNeighbour (edgeNode, true);
					if (processedNeighbour != null) {
						Merge (processedNeighbour, edgeNode);
						Link (processedNeighbour, edgeNode);
						
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
		public NodeVO GetNode (int x, int y)
		{
			return _data [x + y * _width];
		}
		
		private void CreateBranch (NodeVO startNode, List<NodeVO> edgeNodes)
		{
			NodeVO randomNeighbour;
			NodeVO currentNode = startNode;
			
			do {
				//1. if node exists in edge nodes, remove it
				if (edgeNodes.Contains (currentNode))
					edgeNodes.Remove (currentNode);
				
				//1.1 append new edge nodes
				List<NodeVO> notProcessedNeighbours = GetNotProcessedNeighboursOf (currentNode);
				foreach (NodeVO nodeData in notProcessedNeighbours) {
					if (!edgeNodes.Contains (nodeData)) {
						edgeNodes.Add (nodeData);
					}
				}
				
				currentNode.AddFlag (NodeVO.PROCESSED);
				
				//2. go to random direction and get a neighbour
				randomNeighbour = GetRandomNeighbour (currentNode, false);
				
				//3. if it exists (didn't got a dead end) - expand the maze
				if (randomNeighbour != null) {
					Link (currentNode, randomNeighbour);
					
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
		private List<NodeVO> GetNotProcessedNeighboursOf (NodeVO target)
		{
			List<NodeVO> neighbours = new List<NodeVO> ();
			
			for (int i = 0; i < 4; i++) {
				
				int x = target.pos.x + NodeVO.DIRECTIONS [i, 0];
				int y = target.pos.y + NodeVO.DIRECTIONS [i, 1];
				
				if (IsInBounds (x, y)) {
					NodeVO neighbour = GetNode (x, y);
					if (!neighbour.HasFlag (NodeVO.PROCESSED))
						neighbours.Add (neighbour);
				}
			}
			return neighbours;
		}
		
		/**
		 * Checks if provided 2d coordinates are within 1d data array
		 */
		public bool IsInBounds (int x, int y)
		{
			return ((x > -1) && (x < _width) && (y > -1) && (y < _height));
		}
		
		/**
		 * Finds a random neighbour with specified param
		 */
		private NodeVO GetRandomNeighbour (NodeVO target, bool processedNeeded)
		{
			int offset = _rnd.Next (0, 4);
			for (int i = 0; i < 4; i++) {
				int dir = (offset + i) % 4;
				
				int x = target.pos.x + NodeVO.DIRECTIONS [dir, 0];
				int y = target.pos.y + NodeVO.DIRECTIONS [dir, 1];
				
				if (IsInBounds (x, y)) {
					NodeVO neighbour = GetNode (x, y);
					if ((neighbour.HasFlag (NodeVO.PROCESSED) && processedNeeded) || (!neighbour.HasFlag (NodeVO.PROCESSED) && !processedNeeded))
						return neighbour;
				}
			}
			
			return null;
		}
		
		/**
		 * Removes walls between two specified nodes
		 */
		private void Merge (NodeVO from, NodeVO to)
		{
			int dx = to.pos.x - from.pos.x;
			int dy = to.pos.y - from.pos.y;
			
			if (dx != 0) {
				if (dx > 0) {
					to.RemoveWall (NodeVO.DIRECTION_LEFT_IDX);
					from.RemoveWall (NodeVO.DIRECTION_RIGHT_IDX);
				} else {
					to.RemoveWall (NodeVO.DIRECTION_RIGHT_IDX);
					from.RemoveWall (NodeVO.DIRECTION_LEFT_IDX);
				}
			} else if (dy != 0) {
				if (dy > 0) {
					to.RemoveWall (NodeVO.DIRECTION_DOWN_IDX);
					from.RemoveWall (NodeVO.DIRECTION_UP_IDX);
				} else {
					to.RemoveWall (NodeVO.DIRECTION_UP_IDX);
					from.RemoveWall (NodeVO.DIRECTION_DOWN_IDX);
				}
			}
		}
		
		/*
		* Sets previous and next references to each other
		*/
		private void Link(NodeVO previous, NodeVO next){
			next.previousNode = previous;
			previous.nextNode = next;
		}
	}
}

