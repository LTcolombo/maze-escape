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
	//textfields for game state UI
	LerpTextField _scoreText;
	LerpTextField _maxScoreText;
	LerpTextField _movesText;
	LerpTextField _timeBonusText;
				
	//current maze data
	MazeData _mazeData;
	
	//current game state
	GameState _gameState;
	
	//Scene state
	static bool FIRST_LOAD = true;
	static bool INITED = false;
	
	// Use this for initialization
	void Start ()
	{
		if (FIRST_LOAD) {
			FIRST_LOAD = false;
			Application.LoadLevel ("MenuScene");
			FIRST_LOAD = false;
			return;
		}
				
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
	
	void OnPlayerStepComplete (IntPoint cellPosition)
	{
		if (!_gameState.activated)
			Activate ();	
		
		if (_gameState.movesLeft < _gameState.movesLeftCritical) {
			_movesText.color = new Color (0.8f, 0.2f, 0.2f);
		}

		NodeData node = _mazeData.GetNode (cellPosition.x, cellPosition.y);
		
		_gameState.score += (int)((float)node.score * _gameState.timeBonus);
		
		_scoreText.SetValueImmediate (_gameState.score);
		
		if (_gameState.maxScore < _gameState.score) {
			_gameState.maxScore = _gameState.score;
			_maxScoreText.SetValueImmediate (_gameState.maxScore);
		}

		_scoreText.color = new Color (0.55f, 0.55f, 0.55f);
		
		if (_gameState.stuck){
			_gameState.stuck = false;
		}
		
		BroadcastMessage ("DesaturateTileAt", node.pos);
		
		_gameState.movesLeft--;
		_movesText.SetValueImmediate (_gameState.movesLeft);
				
		if (_gameState.movesLeft == 0) {
			
			if (_gameState.maxScore > PlayerPrefs.GetInt ("highscore", 0))
				PlayerPrefs.SetInt ("highscore", _gameState.maxScore);
			
			AnalyticsWrapper.ReportGameLost (_gameState);
			
			Application.LoadLevel ("MenuScene");
			return;
		} 

		BroadcastMessage ("onNodeReached", node);
	}

	void onExit()  {
		_movesText.color = new Color (0.55f, 0.55f, 0.55f);

		_gameState.score += (int)((float)_gameState.movesLeft * _gameState.timeBonus * (_mazeData.config.minScore + _mazeData.config.maxScore) / 2);
		_scoreText.SetValue (_gameState.score);
		_movesText.SetValue (0);
		_gameState.levelNumber ++;

		Invoke ("Next", 0.6f);
	}

	void onStuck(){		
		_scoreText.SetValue (0, _mazeData.config.scoreDrainTime);
		
		_scoreText.color = new Color (0.8f, 0.2f, 0.2f);
		_gameState.stuck = true;
	}
	
	void Next ()
	{	
		if (_gameState.levelNumber > PlayerPrefs.GetInt ("maxlevel", 0))
			PlayerPrefs.SetInt ("maxlevel", _gameState.levelNumber);
	
		_gameState.activated = false;
	
		_mazeData = new MazeData (new MazeConfig (_gameState.levelNumber), cellPosition.x, cellPosition.y);
		
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
	
	void Activate ()
	{
		_gameState.timeBonus = _timeBonusText.GetImmediateValue ();
		_timeBonusText.SetValueImmediate (_gameState.timeBonus);
		_gameState.activated = true;
		DOTween.CompleteAll ();
	}
}
