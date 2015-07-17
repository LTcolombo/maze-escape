using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class GameController : MonoBehaviour
{
	GameObject _HUD;
				
	//current maze data
	MazeData _mazeData;
	
	//current game state
	GameState _gameState;

	//starting point of the maze
	IntPoint _mazeStartPos;
	
	int _scoreOnStuck = 0;
	
	// Use this for initialization
	void Start ()
	{
		_HUD = (GameObject)GameObject.Find("HUD");
	
		_mazeStartPos = new IntPoint (0, 0);
		_gameState.movesLeft = 0;
		_gameState.score = 0;
		_gameState.levelNumber = PlayerPrefs.GetInt ("maxlevel", 0) / 2;
		Next ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	//todo if not activated - drain timebonus;
	//if ended - transfer moves to score in 0.5 seconds
		if (_gameState.state == GameState.STATE_STUCK) {
			_gameState.score -= 1 + (int)(Time.deltaTime * _scoreOnStuck / _mazeData.config.scoreDrainTime);
			if (_gameState.score <= 0) {
				_gameState.score = 0;
				if (_gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
					PlayerPrefs.SetInt ("highscore", _gameState.maxScore);
					
				AnalyticsWrapper.ReportGameLost (_gameState);
				
				Application.LoadLevel ("MenuScene");
			} 
			_HUD.BroadcastMessage("OnGameStateUpdated", _gameState);
		} else {
			if (_gameState.maxScore < _gameState.score) {
				_gameState.maxScore = _gameState.score;
				
				_HUD.BroadcastMessage("OnGameStateUpdated", _gameState);
			}
		}
	}
	
	void OnApplicationPause (bool paused)
	{
		AnalyticsWrapper.ReportGamePaused (_gameState);
	
		if (_gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
			PlayerPrefs.SetInt ("highscore", _gameState.maxScore);
	}
	
	void OnStepComplete (IntPoint pos)
	{
		if (_gameState.state == GameState.STATE_INITED || _gameState.state == GameState.STATE_STUCK)
			_gameState.state = GameState.STATE_ACTIVATED;	
		
		NodeData node = _mazeData.GetNode (pos.x, pos.y);
		
		_gameState.score += (int)((float)node.score * _gameState.timeBonus);
		
		if (_gameState.maxScore < _gameState.score) {
			_gameState.maxScore = _gameState.score;
		}
		
		_gameState.movesLeft--;
				
		if (_gameState.movesLeft == 0) {
			
			if (_gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
				PlayerPrefs.SetInt ("highscore", _gameState.maxScore);
			
			AnalyticsWrapper.ReportGameLost (_gameState);
			
			Application.LoadLevel ("MenuScene");
			return;
		} 

		BroadcastMessage ("onNodeReached", node);
		_HUD.BroadcastMessage("OnGameStateUpdated", _gameState);
		
		if (node.HasFlag (NodeData.SPECIALS_EXIT)) {
			onExit(pos);
			return;
		}
	}

	void onExit (IntPoint pos)
	{
		_gameState.score += (int)((float)_gameState.movesLeft * _gameState.timeBonus * (_mazeData.config.minScore + _mazeData.config.maxScore) / 2);
		_gameState.levelNumber ++;
		_HUD.BroadcastMessage("OnGameStateUpdated", _gameState);

		_mazeStartPos = pos;
		Invoke ("Next", 0.6f);
	}

	void onStuck ()
	{	
		_gameState.state = GameState.STATE_STUCK;
		_scoreOnStuck = _gameState.score;
	}
	
	void Next ()
	{	
		if (_gameState.levelNumber > PlayerPrefs.GetInt ("maxlevel", 0))
			PlayerPrefs.SetInt ("maxlevel", _gameState.levelNumber);
	
		_gameState.state = GameState.STATE_INITED;
	
		_mazeData = new MazeData (new MazeConfig (_gameState.levelNumber), _mazeStartPos.x, _mazeStartPos.y);
		
		ExitDecorator.Apply (_mazeData);
		ScoreDecorator.Apply (_mazeData);
		HiderDecorator.Apply (_mazeData);
		SpeedUpDecorator.Apply (_mazeData);
		RotatorDecorator.Apply (_mazeData);
		
		BroadcastMessage ("UpdateMazeData", _mazeData);
		
		_gameState.timeBonus = _mazeData.config.maxTimeBonus;
		_gameState.movesLeft = (uint)((float)_mazeData.deadEnds [0].GetDistance () * _mazeData.config.maxTimeBonus);
		
		_HUD.BroadcastMessage("OnGameStateUpdated", _gameState);
	}
}
