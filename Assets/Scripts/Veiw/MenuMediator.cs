using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Analytics;
using Utils;
using UnityEngine.SceneManagement;
using Model;

namespace View
{
	public class MenuMediator : ActionInvoker<MazePaceActions>
	{
		private bool _canExit;

		void Start ()
        {
			InvokeAction (MazePaceActions.ResetModels);

            _canExit = false;
			Invoke ("AllowProceedToGame", 1); //todo commands please
		}

		void AllowProceedToGame ()
		{
			Text tipText = (Text)GameObject.Find ("ScreenHUD/TipText").GetComponent<Text> ();
			tipText.text = "TAP ANYWHERE TO PLAY";
			_canExit = true;
		}

		void Update ()
		{
			InvokeActions ();
			if (Application.isEditor && Input.anyKey && _canExit)
				ProceedToGame ();
			
			if (Input.touchCount > 0) {
				bool start = true;
				for (int i = 0; i < Input.touchCount; i++) {
					if (Input.GetTouch (i).phase != TouchPhase.Ended)
						start = false;
				}

				if (start && _canExit)
					ProceedToGame ();
			}		
		}

		void ProceedToGame ()
		{		
			Text tipText = (Text)GameObject.Find ("ScreenHUD/TipText").GetComponent<Text> ();
			tipText.text = "LOADING...";
			_canExit = false;
			InvokeAction (MazePaceActions.NavigateToGame);
		}
	}
}