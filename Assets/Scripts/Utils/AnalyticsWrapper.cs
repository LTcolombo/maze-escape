using Model;
using System.Collections.Generic;
using UnityEngine.Analytics;

namespace Utils
{
	public class AnalyticsWrapper
	{
		public static void ReportGameLost (int levelNumber, GameModel game)
		{
			IDictionary<string, object> eventData = new Dictionary<string, object> ();
			eventData.Add (new KeyValuePair<string, object> ("Number", levelNumber)); 
			eventData.Add (new KeyValuePair<string, object> ("Score", (int)game.score));
			eventData.Add (new KeyValuePair<string, object> ("MaxScore", (int)game.maxScore));
			eventData.Add (new KeyValuePair<string, object> ("MovesLeft", (int)game.movesLeft));
			
			Analytics.CustomEvent ("GameLost", eventData);
		}
		
		public static void ReportGamePaused (int levelNumber, GameModel game)
		{
			IDictionary<string, object> eventData = new Dictionary<string, object> ();
			eventData.Add (new KeyValuePair<string, object> ("Number", levelNumber)); 
			eventData.Add (new KeyValuePair<string, object> ("Score", (int)game.score));
			eventData.Add (new KeyValuePair<string, object> ("MaxScore", (int)game.maxScore));
			eventData.Add (new KeyValuePair<string, object> ("MovesLeft", (int)game.movesLeft));
			Analytics.CustomEvent ("GamePaused", eventData);
		}
		
		public static void ReportReturnToMenu (int levelNumber, GameModel game)
		{
			IDictionary<string, object> eventData = new Dictionary<string, object> ();
			eventData.Add (new KeyValuePair<string, object> ("Number", levelNumber)); 
			eventData.Add (new KeyValuePair<string, object> ("Score", (int)game.score));
			eventData.Add (new KeyValuePair<string, object> ("MaxScore", (int)game.maxScore));
			eventData.Add (new KeyValuePair<string, object> ("MovesLeft", (int)game.movesLeft));
			Analytics.CustomEvent ("GameExit", eventData);
		}
	}
}

