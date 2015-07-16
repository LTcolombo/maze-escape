using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using DG.Tweening;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Analytics;



public class GameController : MonoBehaviour
{
	
	//container for maze and player
	private GameObject _player;
	
	//textfields for game state UI
	private LerpTextField _scoreText;
	private LerpTextField _maxScoreText;
	private LerpTextField _movesText;
	private LerpTextField _timeBonusText;
		
	//view scripts
	private PlayerView _playerView;
		
	//current maze data
	private MazeData _mazeData;
	
	//current game state
	private GameState _gameState;
	
	//Scene state
	private static bool FIRST_LOAD = true;
	private static bool INITED = false;
	
	// Use this for initialization
	void Start ()
	{
		if (FIRST_LOAD) {
			FIRST_LOAD = false;
			Application.LoadLevel ("MenuScene");
			FIRST_LOAD = false;
			return;
		}
				
		_player = (GameObject)Instantiate (Prefabs.PLAYER);
		_player.transform.parent = transform;
		_playerView = _player.GetComponent<PlayerView> ();
		_playerView.onStepComplete += OnPlayerStepComplete;
				
		_scoreText = (LerpTextField)GameObject.Find ("Canvas/ScoreText").GetComponent<LerpTextField> ();
		_maxScoreText = (LerpTextField)GameObject.Find ("Canvas/MaxScoreText").GetComponent<LerpTextField> ();
		_movesText = (LerpTextField)GameObject.Find ("Canvas/MovesText").GetComponent<LerpTextField> ();
		_timeBonusText = (LerpTextField)GameObject.Find ("Canvas/TimeBonusText").GetComponent<LerpTextField> ();
				
		_gameState.movesLeft = 0;
		_gameState.score = 0;
		_gameState.levelNumber = PlayerPrefs.GetInt ("maxlevel", 0) / 2;
		Next ();
		INITED = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!INITED) 
			return;
		_gameState.score = (int)_scoreText.GetImmediateValue ();
		if (_gameState.stuck) {
			if (_gameState.score == 0) {
				
				if (_gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
					PlayerPrefs.SetInt ("highscore", _gameState.maxScore);
					
				AnalyticsWrapper.ReportGameLost (_gameState);
					
				Application.LoadLevel ("MenuScene");
			} 
		} else {
			if (_gameState.maxScore < _gameState.score) {
				_gameState.maxScore = _gameState.score;
				_maxScoreText.SetValueImmediate (_gameState.maxScore);
			}
		}
	}
	
	void OnApplicationPause (bool paused)
	{
		AnalyticsWrapper.ReportGamePaused (_gameState);
	
		if (_gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
			PlayerPrefs.SetInt ("highscore", _gameState.maxScore);
	}
	
	private void OnPlayerStepComplete ()
	{
		if (!_gameState.activated)
			Activate ();	
		
		if (_gameState.movesLeft < _gameState.movesLeftCritical) {
			_movesText.color = new Color (0.8f, 0.2f, 0.2f);
		}
		
		NodeData node = _mazeData.GetNode (_playerView.cellX, _playerView.cellY);
		
		if (node.HasFlag (NodeData.SPECIALS_EXIT)) {
			_movesText.color = new Color (0.55f, 0.55f, 0.55f);
			
			BroadcastMessage ("DesaturateTileAt", node.pos);
			_gameState.score += (int)((float)node.score * _gameState.timeBonus);
			_gameState.score += (int)((float)_gameState.movesLeft * _gameState.timeBonus * (_mazeData.config.minScore + _mazeData.config.maxScore) / 2);
			_scoreText.SetValue (_gameState.score);
			_movesText.SetValue (0);
			_gameState.levelNumber ++;
			
			_player.SetActive (false);
			Invoke ("Next", 0.6f);
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
			BroadcastMessage ("ShowWalls", false);
		}
		
		if (node.HasFlag (NodeData.SPECIALS_SHOW_WALLS)) {
			BroadcastMessage ("ShowWalls", true);
		}
		
		int rotateBy = 0;
		
		_gameState.movesLeft--;
		_movesText.SetValueImmediate (_gameState.movesLeft);
		if (_gameState.movesLeft == 0) {
			
			if (_gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
				PlayerPrefs.SetInt ("highscore", _gameState.maxScore);
				
			AnalyticsWrapper.ReportGameLost (_gameState);
				
			Application.LoadLevel ("MenuScene");
			return;
		} 
		
		if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CW)) {
			rotateBy = 1;
		}
		
		if (node.HasFlag (NodeData.SPECIALS_ROTATOR_CCW)) {
			rotateBy = -1;
		}
		
		if (!node.HasWall (_playerView.directionIdx) && (!_playerView.didJustMove || rotateBy == 0)) {
			rotateBy = 0;
			BroadcastMessage ("DesaturateTileAt", node.pos);
			_gameState.score += (int)((float)node.score * _gameState.timeBonus);
			_scoreText.SetValueImmediate (_gameState.score);
			
			if (_gameState.maxScore < _gameState.score) {
				_gameState.maxScore = _gameState.score;
				_maxScoreText.SetValueImmediate (_gameState.maxScore);
			}
			
			node.score = 0;
			_playerView.Next (moveTime, rotateBy);
			
			_scoreText.color = new Color (0.55f, 0.55f, 0.55f);
			
			if (_gameState.stuck){
				_gameState.stuck = false;
			}
		} else {
			
			_playerView.Next (-1, rotateBy);

			if (rotateBy == 0) {
				_scoreText.SetValue (0, _mazeData.config.scoreDrainTime);
				
				_scoreText.color = new Color (0.8f, 0.2f, 0.2f);
				_gameState.stuck = true;
			}
		}
	}
		
	private void Next ()
	{	
		if (_gameState.levelNumber > PlayerPrefs.GetInt ("maxlevel", 0))
			PlayerPrefs.SetInt ("maxlevel", _gameState.levelNumber);
	
		_gameState.activated = false;
	
		_mazeData = new MazeData (new MazeConfig (_gameState.levelNumber), _playerView.cellX, _playerView.cellY);
		
		ExitDecorator.Apply (_mazeData);
		ScoreDecorator.Apply (_mazeData);
		HiderDecorator.Apply (_mazeData);
		SpeedUpDecorator.Apply (_mazeData);
		RotatorDecorator.Apply (_mazeData);
		
		BroadcastMessage("UpdateMazeData", _mazeData);
		
		_gameState.movesLeft = (uint)((float)_mazeData.deadEnds [0].GetDistance () * _mazeData.config.maxTimeBonus);
		_gameState.movesLeftCritical = _gameState.movesLeft / 10;
		_movesText.color = new Color (0.55f, 0.55f, 0.55f);
		
		_timeBonusText.SetValueImmediate (_mazeData.config.maxTimeBonus);
		_timeBonusText.SetValue (_mazeData.config.minTimeBonus, _mazeData.config.bonusTime);
		
		_scoreText.SetValueImmediate (_gameState.score);
		_movesText.SetValueImmediate (_gameState.movesLeft);
		
		_player.SetActive (true);
	}
	
	private void Activate ()
	{
		_gameState.timeBonus = _timeBonusText.GetImmediateValue ();
		_timeBonusText.SetValueImmediate (_gameState.timeBonus);
		_gameState.activated = true;
		DOTween.CompleteAll ();
	}
}
