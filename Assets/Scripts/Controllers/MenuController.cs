using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Analytics;
using Utils;
using UnityEngine.SceneManagement;

namespace Controllers {
	public class MenuController : MonoBehaviour
	{

		private bool _canExit;
		
		
		// Use this for initialization
		void Start ()
		{
			Prefabs.Init ();
			
			Text bestScoreText = (Text)GameObject.Find ("Canvas/BestScoreText").GetComponent<Text> ();
			bestScoreText.text = "BEST SCORE: " + PlayerPrefs.GetInt ("highscore", 0);
			
			_canExit = false;
			Invoke("AllowExit", 1);
		}
		
		void AllowExit ()
		{
			Text tipText = (Text)GameObject.Find ("Canvas/TipText").GetComponent<Text> ();
			tipText.text = "TAP ANYWHERE TO PLAY";
			_canExit = true;
		}
		
		// Update is called once per frame
		void Update ()
		{	
			if (Application.isEditor && Input.anyKey && _canExit)
				ExitToGame();
			
			if (Input.touchCount > 0) {
				bool start = true;
				for (int i = 0; i < Input.touchCount; i++) {
					if (Input.GetTouch (i).phase != TouchPhase.Ended) 
						start = false;
				}
				
				if (start && _canExit) 
					ExitToGame();
			}		
		}

		void ExitToGame() {		
			Text tipText = (Text)GameObject.Find ("Canvas/TipText").GetComponent<Text> ();
			tipText.text = "LOADING...";
			_canExit = false;
			SceneManager.LoadScene ("GameScene");
		}
	}
}