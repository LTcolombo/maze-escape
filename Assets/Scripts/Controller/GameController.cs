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
	private LerpTextField _scoreText;
	private LerpTextField _maxScoreText;
	private LerpTextField _movesText;
	private LerpTextField _timeBonusText;
		
	//view scripts
	private MazeView _mazeView;
	private PlayerView _playerView;
		
	//current maze data
	private MazeData _mazeData;
	
	//current game state
	private int _levelNumber = 0;
	private int _score;
	private int _maxScore;
	private uint _movesLeft;
	private uint _movesLeftCritical;
	private float _timeBonus;
	private bool _activated;
	private bool _stuck;

	//time whick takes off the scure while stuck
	private static float STUCK_TIME = 2.0f;
	
	//Scene state
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
				
		_scoreText = (LerpTextField)GameObject.Find ("Canvas/ScoreText").GetComponent<LerpTextField> ();
		_maxScoreText = (LerpTextField)GameObject.Find ("Canvas/MaxScoreText").GetComponent<LerpTextField> ();
		_movesText = (LerpTextField)GameObject.Find ("Canvas/MovesText").GetComponent<LerpTextField> ();
		_timeBonusText = (LerpTextField)GameObject.Find ("Canvas/TimeBonusText").GetComponent<LerpTextField> ();
				
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
		_score = (int)_scoreText.GetImmediateValue ();
		if (_stuck) {
		
		
			if (_score == 0) {
				
				if (_maxScore > PlayerPrefs.GetInt ("highscore", 0))
					PlayerPrefs.SetInt ("highscore", _maxScore);
					
				IDictionary<string, object> eventData = new Dictionary<string, object> ();
				eventData.Add (new KeyValuePair<string, object> ("Number", _levelNumber));
				eventData.Add (new KeyValuePair<string, object> ("Score", _score));
				eventData.Add (new KeyValuePair<string, object> ("MaxScore", _maxScore));
				eventData.Add (new KeyValuePair<string, object> ("MovesLeft", _movesLeft));
				UnityAnalytics.CustomEvent ("GameLost", eventData);
					
				Application.LoadLevel ("MenuScene");
			} 
		} else {
			if (_maxScore < _score) {
				_maxScore = _score;
				_maxScoreText.SetValueImmediate (_maxScore);
			}
		}
	}
	
	void OnApplicationPause (bool paused)
	{
		IDictionary<string, object> eventData = new Dictionary<string, object> ();
		eventData.Add (new KeyValuePair<string, object> ("Number", _levelNumber));
		eventData.Add (new KeyValuePair<string, object> ("Score", _score));
		eventData.Add (new KeyValuePair<string, object> ("MaxScore", _maxScore));
		eventData.Add (new KeyValuePair<string, object> ("MovesLeft", _movesLeft));
		UnityAnalytics.CustomEvent ("GamePaused", eventData);
	
		if (_maxScore > PlayerPrefs.GetInt ("highscore", 0))
			PlayerPrefs.SetInt ("highscore", _maxScore);
	}
	
	private void OnPlayerStepComplete ()
	{
		if (!_activated)
			Activate ();
	
		NodeData node = _mazeData.GetNode (_playerView.cellX, _playerView.cellY);
		
		_scoreText.SetValueImmediate (_score);
		_movesText.SetValueImmediate (_movesLeft);
		
		if (node.HasFlag (NodeData.SPECIALS_EXIT)) {
		    _movesText.color = new Color (0.55f, 0.55f, 0.55f);
			
			_score += (int)((float)_movesLeft * _timeBonus);
			_scoreText.SetValue (_score);
			_movesText.SetValue (0);
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
		
		if (_movesLeft < _movesLeftCritical) {
			_movesText.color = new Color (0.8f, 0.2f, 0.2f);
		}
		
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
			_scoreText.SetValueImmediate (_score);
			
			if (_maxScore < _score) {
				_maxScore = _score;
				_maxScoreText.SetValueImmediate (_maxScore);
			}
			
			node.score = 0;
			_playerView.Next (moveTime, rotateBy);
			
			_scoreText.color = new Color (0.55f, 0.55f, 0.55f);
			_stuck = false;
		} else {
			
			_playerView.Next (-1, rotateBy);

			if (rotateBy == 0) {
				_scoreText.SetValue (0, STUCK_TIME);
				
				_scoreText.color = new Color (0.8f, 0.2f, 0.2f);
				_stuck = true;
			}
		}
	}
		
	private void Next ()
	{	
		if (_levelNumber > PlayerPrefs.GetInt ("maxlevel", 0))
			PlayerPrefs.SetInt ("maxlevel", _levelNumber);
	
		_activated = false;
	
		_mazeData = new MazeData (new MazeConfig (_levelNumber), _playerView.cellX, _playerView.cellY);
		
		ExitDecorator.Apply(_mazeData);
		ScoreDecorator.Apply (_mazeData);
		HiderDecorator.Apply (_mazeData);
		SpeedUpDecorator.Apply (_mazeData);
		RotatorDecorator.Apply (_mazeData);
		
		_container.transform.DOMove (new Vector2 (
			-(_mazeData.config.width - 1) * MazeView.NODE_SIZE / 2, 
			-(_mazeData.config.height - 1) * MazeView.NODE_SIZE / 2
		), MazeView.TRANSITION_TIME);
		
		_mazeView.UpdateMazeData (_mazeData);
		
		_timeBonusText.SetValueImmediate (_mazeData.config.maxTimeBonus);
		_timeBonusText.SetValue (_mazeData.config.minTimeBonus, _mazeData.config.bonusTime);
	}
	
	private void Activate ()
	{
		_timeBonus = _timeBonusText.GetImmediateValue ();
		_timeBonusText.SetValueImmediate (_timeBonus);
			
		_movesLeft = (uint)((float)_mazeData.deadEnds [0].GetDistance () * _mazeData.config.maxTimeBonus);
		_movesLeftCritical = _movesLeft / 10;
		_movesText.color = new Color (0.55f, 0.55f, 0.55f);
		_activated = true;
	}
}
