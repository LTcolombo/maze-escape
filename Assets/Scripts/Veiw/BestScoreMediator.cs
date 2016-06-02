using UnityEngine.UI;
using UnityEngine;
using Model;

namespace View
{
	public class BestScoreMediator : MonoBehaviour
	{
		protected void Start ()
		{
			Text target = GetComponent<Text> ();
			if (target != null)
				target.text = "BEST SCORE: " + GameModel.Instance().maxScore;
			else
				Debug.Log ("No text component found!");
		}
	}
}

