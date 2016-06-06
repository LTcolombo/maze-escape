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
		private const int HEART_GAP = 20;

		private Stack<GameObject> _instances;

		private int _previousLives = -1;

		protected void Start ()
		{
			_instances = new Stack<GameObject> ();
		}

		protected void Update ()
		{		
			SetLives (LivesModel.Instance ().GetLives ());
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
				heart.transform.localPosition = new Vector2 (startOffset + HEART_GAP * i, 0);
				i++;
			}

			_previousLives = value;
		}
	}
}

