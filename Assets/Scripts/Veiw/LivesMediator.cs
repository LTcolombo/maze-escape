using UnityEngine.UI;
using Model;
using UnityEngine;
using Utils;
using System.Collections.Generic;
using System;

namespace View
{
	public class LivesMediator : MonoBehaviour
	{
		private const int HEART_GAP = 80;

		private Text _timeUntilLifeRestoredText;
		private Stack<GameObject> _instances;

		private int _previousTime;
		private int _previousLives;

		protected void Start ()
		{
			_instances = new Stack<GameObject> ();
			_timeUntilLifeRestoredText = (Text)GameObject.Find ("Canvas/TimeUntilLifeRestored").GetComponent<Text> ();
		}

		protected void Update ()
		{		
			SetTime (LivesModel.Instance ().NextLifeIn ());
			SetLives (LivesModel.Instance ().GetLives ());
		}

		void SetTime (int value)
		{
			if (value > 0)
				_timeUntilLifeRestoredText.text = String.Format ("{0:00}:{1:00}", (value / 60), (value % 60));
			else
				_timeUntilLifeRestoredText.text = "MAX";
		}

		void SetLives (int value)
		{
			if (_previousLives == value)
				return;

			while (_instances.Count > value) {
				Destroy (_instances.Pop ());
			}

			while (_instances.Count < value) {
				GameObject heartInstance = (GameObject)Instantiate (PrefabLib.HEART, new Vector2 (0, 0), Quaternion.identity);
				heartInstance.transform.parent = transform;
				_instances.Push (heartInstance);
			}

			float startOffset = -HEART_GAP * (float)(_instances.Count - 1) / 2;

			int i = 0;
			foreach (GameObject heart in _instances) {
				heart.transform.position = new Vector2(startOffset + HEART_GAP * i, -260);
				i++;
			}

			_previousLives = value;
		}
	}
}

