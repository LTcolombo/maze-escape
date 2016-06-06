using UnityEngine.UI;
using Model;
using UnityEngine;
using Utils;
using System.Collections.Generic;
using System;

namespace View
{
	public class TimeUntilLifeRestoredMediator : MonoBehaviour
	{
		private Text _target;

		private int _previousTime = -1;

		protected void Start ()
		{
			_target = GetComponent<Text> ();
		}

		protected void Update ()
		{		
			SetTime (LivesModel.Instance ().NextLifeIn ());
		}

		void SetTime (int value)
		{
			if (_previousTime == value)
				return;

			if (value > 0)
				_target.text = String.Format ("{0:00}:{1:00}", (value / 60), (value % 60));
			else
				_target.text = "MAX";

			_previousTime = value;
		}
	}
}

