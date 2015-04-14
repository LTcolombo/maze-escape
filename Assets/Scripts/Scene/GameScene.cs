using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using DG.Tweening;
using System;

public class GameScene : MonoBehaviour
{
		public GameObject maze;
		public GameObject player;
		
		//view scripts
		private MazeView _mazeView;
		private PlayerView _playerView;
		
		//current maze data
		private MazeData _mazeData;
	
		// Use this for initialization
		void Start ()
		{
				DOTween.Init (false, true, LogBehaviour.ErrorsOnly);
		
				var mazeObject = (GameObject)Instantiate (maze);
				_mazeView = mazeObject.GetComponent<MazeView> ();
				
				var playerObject = (GameObject)Instantiate (player);
				_playerView = playerObject.GetComponent<PlayerView> ();
				_playerView.onStepComplete += OnPlayerStepComplete;
				Next ();
		}
	
		void OnPlayerStepComplete ()
		{
				if (!_mazeData.IsInBounds (_playerView.cellX + MazeData.DIRECTIONS [_playerView.directionIdx, 0], _playerView.cellY + MazeData.DIRECTIONS [_playerView.directionIdx, 1]))
						return;
		
				NodeData node = _mazeData.GetNode (_playerView.cellX, _playerView.cellY);
				
				Debug.LogFormat ("walls {0} at x:{1}, y:{2} ", Convert.ToString (node.walls, 2), _playerView.cellX, _playerView.cellY);
				
				
				if (!node.HasWall (_playerView.directionIdx))
						_playerView.Next ();
		}
			
		public void OnLeft ()
		{
				_playerView.OnLeft ();
		}
			
		public void OnRight ()
		{
				_playerView.OnRight ();
		}
			
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.N)) {
						Next ();
				}
		}
		
		void Next ()
		{
				_mazeData = MazeGenerator.Generate (9, 9);
				_mazeView.UpdateMazeData (_mazeData);
		}
}
