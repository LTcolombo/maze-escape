using UnityEngine;
using System.Collections;
using Models;

namespace Views
{
	public class TutorialMediator : MonoBehaviour
	{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		public void UpdateMazeData (MazeModel mazeData)
		{
			enabled = mazeData.config.isTutorial;
			onNodeReached (mazeData.startingNode);
		}
	
		// Update is called once per frame
		void onNodeReached (NodeModel node)
		{
			//check if there are two nodes ahead to define needed direction
			if (node.nextNode != null && node.nextNode.nextNode != null) {
				Debug.Log (node.nextNode.GetDirectionTowards (node.nextNode.nextNode));
			}
		}
	}
}

