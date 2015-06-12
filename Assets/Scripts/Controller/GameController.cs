using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using DG.Tweening;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;

public class GameController : MonoBehaviour
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
	private int _levelNumber = 0;
		
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

	//scores to to takeoff on ueach update when stuck. approx should drain all score in 1 secon
	private int _reduceValue;
	private static bool FIRST_LOAD = true;
	private static bool INITED = false;
	// Use this for initialization
	void Start ()
	{
		
		if (!Debug.isDebugBuild) {
			GetComponent<Console> ().enabled = false;
			GetComponent<HUDFPS> ().enabled = false;
		}
		
		if (FIRST_LOAD) {
			FIRST_LOAD = false;
			Application.LoadLevel ("MenuScene");
			FIRST_LOAD = false;
			return;
		}
		DOTween.CompleteAll ();
		Camera.main.backgroundColor = new Color (0.0f, 0.0f, 0.0f);
		Camera.main.DOColor (new Color (0.4f, 0.4f, 0.4f), 0.5f);
				
		_container = GameObject.Find ("GameContainer");
		
		GameObject mazeObject = (GameObject)Instantiate (Prefabs.MAZE);
		mazeObject.transform.parent = _container.transform;
		_mazeView = mazeObject.GetComponent<MazeView> ();
				
		GameObject playerObject = (GameObject)Instantiate (Prefabs.PLAYER);
		playerObject.transform.parent = _container.transform;
		_playerView = playerObject.GetComponent<PlayerView> ();
		_playerView.onStepComplete += OnPlayerStepComplete;
				
		scoreText = (Text)GameObject.Find ("Canvas/ScoreText").GetComponent<Text> ();
		maxScoreText = (Text)GameObject.Find ("Canvas/MaxScoreText").GetComponent<Text> ();
		movesText = (Text)GameObject.Find ("Canvas/MovesText").GetComponent<Text> ();
				
		_movesLeft = 0;
		_score = 0;
		_levelNumber = PlayerPrefs.GetInt ("maxlevel", 0) / 2;
		Next ();
		INITED = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!INITED) 
			return;
		if (_stuck) {
			scoreText.color = new Color (0.8f, 0.2f, 0.2f);
			if (_score < _reduceValue) {
				
				if (_maxScore > PlayerPrefs.GetInt ("highscore", 0))
					PlayerPrefs.SetInt ("highscore", _maxScore);
					
				
				IDictionary<string, object> eventData = new Dictionary<string, object> ();
				eventData.Add (new KeyValuePair<string, object> ("Number", _levelNumber));
				eventData.Add (new KeyValuePair<string, object> ("Score", _score));
				eventData.Add (new KeyValuePair<string, object> ("MaxScore", _maxScore));
				eventData.Add (new KeyValuePair<string, object> ("MovesLeft", _movesLeft));
				UnityAnalytics.CustomEvent ("GameLost", eventData);
					
				Application.LoadLevel ("MenuScene");
			} else {
				_score -= _reduceValue;
				scoreText.text = "SCORE: " + _score;
			}
		} else
			if (_increaseValue == 0)
				scoreText.color = new Color (0.55f, 0.55f, 0.55f);
		else {
			if (_increaseValue > 0) {
				scoreText.color = new Color (0.2f, 0.8f, 0.2f);

				if (_increaseValue > _movesLeft)
					_increaseValue = _movesLeft;
			} else {
				scoreText.color = new Color (0.8f, 0.2f, 0.2f);	
				
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
		
		IDictionary<string, object> eventData = new Dictionary<string, object> ();
		eventData.Add (new KeyValuePair<string, object> ("Number", _levelNumber));
		eventData.Add (new KeyValuePair<string, object> ("Score", _score));
		eventData.Add (new KeyValuePair<string, object> ("MaxScore", _maxScore));
		eventData.Add (new KeyValuePair<string, object> ("MovesLeft", _movesLeft));
		UnityAnalytics.CustomEvent ("GameExited", eventData);
	
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
			movesText.color = new Color (0.55f, 0.55f, 0.55f);
			_levelNumber ++;
			Next ();
			return;
		}
		
		float moveTime = _mazeData.config.moveTime;
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_UP)) {
			if (_playerView.directionIdx == NodeData.DIRECTION_UP_IDX)
				moveTime /= 2;
			
			if (_playerView.directionIdx == NodeData.DIRECTION_DOWN_IDX)
				moveTime *= 2;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_RIGHT)) {
			if (_playerView.directionIdx == NodeData.DIRECTION_RIGHT_IDX)
				moveTime /= 2;
			
			if (_playerView.directionIdx == NodeData.DIRECTION_LEFT_IDX)
				moveTime *= 2;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_DOWN)) {
			if (_playerView.directionIdx == NodeData.DIRECTION_DOWN_IDX)
				moveTime /= 2;
			
			if (_playerView.directionIdx == NodeData.DIRECTION_UP_IDX)
				moveTime *= 2;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_SPEEDUP_LEFT)) {
			if (_playerView.directionIdx == NodeData.DIRECTION_LEFT_IDX)
				moveTime /= 2;
			
			if (_playerView.directionIdx == NodeData.DIRECTION_RIGHT_IDX)
				moveTime *= 2;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_HIDE_WALLS)) {
			_mazeView.ShowWalls (false);
		}
		
		if (node.HasFlag (NodeData.SPECIALS_SHOW_WALLS)) {
			_mazeView.ShowWalls (true);
		}
		
		int rotateBy = 0;
		
		_movesLeft--;
		if (_movesLeft == 0) {
			
			if (_maxScore > PlayerPrefs.GetInt ("highscore", 0))
				PlayerPrefs.SetInt ("highscore", _maxScore);
				
			
			IDictionary<string, object> eventData = new Dictionary<string, object> ();
			eventData.Add (new KeyValuePair<string, object> ("Number", _levelNumber));
			eventData.Add (new KeyValuePair<string, object> ("Score", _score));
			eventData.Add (new KeyValuePair<string, object> ("MaxScore", _maxScore));
			eventData.Add (new KeyValuePair<string, object> ("MovesLeft", _movesLeft));
			UnityAnalytics.CustomEvent ("GameLost", eventData);
				
			Application.LoadLevel ("MenuScene");
			return;
		} 
		
		if (_movesLeft < _movesLeftCritical)
			movesText.color = new Color (0.8f, 0.2f, 0.2f);
		
		if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CW)) {
			rotateBy = 1;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CCW)) {
			rotateBy = -1;
		}
		
		if (!node.HasWall (_playerView.directionIdx) && (!_playerView.didJustMove || rotateBy == 0)) {
			rotateBy = 0;
			_mazeView.DesaturateTileAt (_playerView.cellX, _playerView.cellY);
			_score += node.score;
			
			if (_maxScore < _score) {
				_maxScore = _score;
				maxScoreText.text = "MAX SCORE: " + _maxScore;
			}
			
			node.score = 0;
			_playerView.Next (moveTime, rotateBy);
			_stuck = false;
		} else {
			
			_playerView.Next (-1, rotateBy);

			if (rotateBy == 0) {
				_reduceValue = (int)((float)_score * Time.deltaTime);
				if (_reduceValue < 1)
					_reduceValue = 1;

				_stuck = true;
			}
		}
	}
		
	private void Next ()
	{	
		if (_levelNumber > PlayerPrefs.GetInt ("maxlevel", 0))
			PlayerPrefs.SetInt ("maxlevel", _levelNumber);
	
		_activated = false;
	
		if (_movesLeft > 0) {
			_increaseValue = (uint)((float)_movesLeft * Time.deltaTime * 2);
			if (_increaseValue == 0)
				_increaseValue = 1;
		} else
			_increaseValue = 0;
	
		_mazeData = new MazeData (new MazeConfig(_levelNumber), _playerView.cellX, _playerView.cellY);
		
		ScoreDecorator.Apply (_mazeData);
		SpeedUpDecorator.Apply (_mazeData);
		RotatorDecorator.Apply (_mazeData);
		HiderDecorator.Apply (_mazeData);
		
		_container.transform.DOMove (new Vector2 (
			-(_mazeData.config.width - 1) * MazeView.NODE_SIZE / 2, 
			-(_mazeData.config.height - 1) * MazeView.NODE_SIZE / 2
		), MazeView.TRANSITION_TIME);
		
		_mazeView.UpdateMazeData (_mazeData);
		
		//_playerView.InvokeAutostartIn (1);
	}

	private void Activate ()
	{
		_increaseValue = 0;
		_score += (int)_movesLeft;

		_movesLeft = (uint)((float)_mazeData.deadEnds[0].GetDistance () * _mazeData.config.bonusRate);
		_movesLeftCritical = _movesLeft / 10;
		_activated = true;
	}
}
