using UnityEngine.UI;
using UnityEngine;

namespace View
{
	public class BestScoreMediator : MonoBehaviour
	{
		protected void Start ()
		{
			Text target = GetComponent<Text> ();
			if (target != null)
				target.text = "BEST SCORE: " + PlayerPrefs.GetInt ("highscore", 0);
			else
				Debug.Log ("No text component found!");
		}
	}
}

