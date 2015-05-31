using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
	
	//container for maze and player
	private GameObject _container;
	private GameObject _maze;
	private GameObject _player;
	
	//textfields for game state UI
	private Text scoreText;
	private Text maxScoreText;
	private Text movesText;
		
	//view scripts
	private MazeView _mazeView;
	private PlayerView _playerView;
		
	//current maze data
	private MazeData _mazeData;
	
	//current game state
	private int _score;
	private int _maxScore;
	private uint _movesLeft;
	private uint _movesLeftCritical;
	private bool _activated;
	private bool _stuck;
	
	//score to add after previous interation, depending on bonus moves
	private uint _increaseValue;

	//scores to to takeoff on ueach update when stuck. approx should drain all score in 1 second
	private int _reduceValue;
	
	// Use this for initialization
	void Start ()
	{
		DOTween.CompleteAll ();
		Camera.main.backgroundColor = new Color (0.17f, 0.17f, 0.17f);
		Camera.main.DOColor (new Color (0.92f, 0.92f, 0.86f), 0.5f);
				
		_container = GameObject.Find ("GameContainer");
		
		var mazeObject = (GameObject)Instantiate (Resources.Load ("Prefabs/Maze"));
		mazeObject.transform.parent = _container.transform;
		_mazeView = mazeObject.GetComponent<MazeView> ();
				
		var playerObject = (GameObject)Instantiate (Resources.Load ("Prefabs/Player"));
		playerObject.transform.parent = _container.transform;
		_playerView = playerObject.GetComponent<PlayerView> ();
		_playerView.onStepComplete += OnPlayerStepComplete;
				
		scoreText = (Text)GameObject.Find ("Canvas/ScoreText").GetComponent<Text> ();
		maxScoreText = (Text)GameObject.Find ("Canvas/MaxScoreText").GetComponent<Text> ();
		movesText = (Text)GameObject.Find ("Canvas/MovesText").GetComponent<Text> ();
				
		_movesLeft = 0;
		_score = 0;
		Next ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_stuck) {
			scoreText.color = new Color (1.0f, 0.0f, 0.0f);
			if (_score < _reduceValue)
				Application.LoadLevel ("MenuScene");
			else {
				_score -= _reduceValue;
				scoreText.text = "SCORE: " + _score;
			}
		} else
			if (_increaseValue == 0)
			scoreText.color = new Color (0.761f, 0.761f, 0.668f);
		else {
			if (_increaseValue > 0) {
				scoreText.color = new Color (0.0f, 0.8f, 0.0f);

				if (_increaseValue > _movesLeft)
					_increaseValue = _movesLeft;
			} else {
				scoreText.color = new Color (1.0f, 0.0f, 0.0f);	
				
				if (_increaseValue < _movesLeft)
					_increaseValue = _movesLeft;
			}
			
			_score += (int)_increaseValue;
			
			if (_maxScore < _score) {
				_maxScore = _score;
				maxScoreText.text = "MAX SCORE: " + _maxScore;
			}
			
			_movesLeft -= _increaseValue;
			scoreText.text = "SCORE: " + _score;
			movesText.text = "MOVES: " + _movesLeft;
		}
	}
	
	void OnApplicationPause (bool paused)
	{
		if (_maxScore > PlayerPrefs.GetInt ("highscore", 0))
			PlayerPrefs.SetInt ("highscore", _maxScore);
	}
	
	private void OnPlayerStepComplete ()
	{
		if (!_activated)
			Activate ();
	
		NodeData node = _mazeData.GetNode (_playerView.cellX, _playerView.cellY);
		
		scoreText.text = "SCORE: " + _score;
		movesText.text = "MOVES: " + _movesLeft;
		
		if (node.HasFlag (NodeData.SPECIALS_EXIT)) {
			movesText.color = new Color (0.761f, 0.761f, 0.668f);
			Next ();
			return;
		}
		
		_playerView.speed = 1;
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_UP)) {
			if (_playerView.directionIdx == NodeData.DIRECTION_UP_IDX)
				_playerView.speed = 2;
			
			if (_playerView.directionIdx == NodeData.DIRECTION_DOWN_IDX)
				_playerView.speed = 0.5f;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_RIGHT)) {
			if (_playerView.directionIdx == NodeData.DIRECTION_RIGHT_IDX)
				_playerView.speed = 2;
			
			if (_playerView.directionIdx == NodeData.DIRECTION_LEFT_IDX)
				_playerView.speed = 0.5f;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_DOWN)) {
			if (_playerView.directionIdx == NodeData.DIRECTION_DOWN_IDX)
				_playerView.speed = 2;
			
			if (_playerView.directionIdx == NodeData.DIRECTION_UP_IDX)
				_playerView.speed = 0.5f;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_LEFT)) {
			if (_playerView.directionIdx == NodeData.DIRECTION_LEFT_IDX)
				_playerView.speed = 2;
			
			if (_playerView.directionIdx == NodeData.DIRECTION_RIGHT_IDX)
				_playerView.speed = 0.5f;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_HIDE_WALLS)) {
			_mazeView.ShowWalls (false);
		}
		
		if (node.HasFlag (NodeData.SPECIALS_SHOW_WALLS)) {
			_mazeView.ShowWalls (true);
		}
		
		_playerView.ddirection = 0;
		
		_movesLeft--;
		if (_movesLeft == 0) {
			
			if (_maxScore > PlayerPrefs.GetInt ("highscore", 0))
				PlayerPrefs.SetInt ("highscore", _maxScore);
				
			Application.LoadLevel ("MenuScene");
			return;
		} 
		
		if (_movesLeft < _movesLeftCritical)
			movesText.color = new Color (1.0f, 0.0f, 0.0f);
		
		if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CW)) {
			_playerView.ddirection = 1;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CCW)) {
			_playerView.ddirection = -1;
		}
		
		if (!node.HasWall (_playerView.directionIdx) && (!_playerView.moved || _playerView.ddirection == 0)) {
			_playerView.ddirection = 0;
			_mazeView.DesaturateTileAt (_playerView.cellX, _playerView.cellY);
			_score += node.score;
			
			if (_maxScore < _score) {
				_maxScore = _score;
				maxScoreText.text = "MAX SCORE: " + _maxScore;
			}
			
			node.score = 0;
			_playerView.Next (true);
			_stuck = false;
		} else {
			
			_playerView.Next (false);

			if (_playerView.ddirection == 0) {
				_reduceValue = (int)((float)_score * Time.deltaTime);
				if (_reduceValue < 1)
					_reduceValue = 1;

				_stuck = true;
			}
		}
	}
		
	private void Next ()
	{
		_activated = false;
	
		if (_movesLeft > 0) {
			_increaseValue = (uint)((float)_movesLeft * Time.deltaTime * 2);
			if (_increaseValue == 0)
				_increaseValue = 1;
		} else
			_increaseValue = 0;
	
		_mazeData = new MazeData (getNextMazeConfig (), _playerView.cellX, _playerView.cellY);
		
		ScoreDecorator.Apply (_mazeData);
		SpeedUpDecorator.Apply (_mazeData);
		RotatorDecorator.Apply (_mazeData);
		HiderDecorator.Apply (_mazeData);
		
		_container.transform.position = new Vector2 (
			-(_mazeData.config.width - 1) * MazeView.NODE_SIZE / 2, 
			-(_mazeData.config.height - 1) * MazeView.NODE_SIZE / 2
		);
		
		_mazeView.UpdateMazeData (_mazeData);
		
		//_playerView.InvokeAutostartIn (1);
	}

//move to model
	MazeConfig getNextMazeConfig ()
	{
		MazeConfig config = new MazeConfig ();
		config.width = 10;
		config.height = 10;
		
		config.minScore = 1;
		config.maxScore = 4;
		
		config.speedUpsCount = 2;
		config.rotatorsCount = 6;
		config.hidersCount = 1;
		
		return config;
	}
	
	private void Activate ()
	{
		_increaseValue = 0;
		_score += (int)_movesLeft;
		_movesLeft = _mazeData.movesQuota;
		_movesLeftCritical = _movesLeft / 10;
		_activated = true;
	}
}
