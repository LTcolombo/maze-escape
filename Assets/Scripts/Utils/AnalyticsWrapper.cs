using Model;
using System.Collections.Generic;
using UnityEngine.Analytics;

namespace Utils
{
	public class AnalyticsWrapper
	{
		public static void ReportGameLost (GameModel gameState)
		{
			IDictionary<string, object> eventData = new Dictionary<string, object> ();
//			eventData.Add (new KeyValuePair<string, object> ("Number", gameState.levelNumber)); todo
			eventData.Add (new KeyValuePair<string, object> ("Score", gameState.score));
			eventData.Add (new KeyValuePair<string, object> ("MaxScore", gameState.maxScore));
			eventData.Add (new KeyValuePair<string, object> ("MovesLeft", gameState.movesLeft));
			
			Analytics.CustomEvent ("GameLost", eventData);
		}
		
		public static void ReportGamePaused (GameModel gameState)
		{
			IDictionary<string, object> eventData = new Dictionary<string, object> ();
//			eventData.Add (new KeyValuePair<string, object> ("Number", gameState.levelNumber)); todo
			eventData.Add (new KeyValuePair<string, object> ("Score", gameState.score));
			eventData.Add (new KeyValuePair<string, object> ("MaxScore", gameState.maxScore));
			eventData.Add (new KeyValuePair<string, object> ("MovesLeft", gameState.movesLeft));
			Analytics.CustomEvent ("GamePaused", eventData);
		}
		
		public static void ReportReturnToMenu (GameModel gameState)
		{
			IDictionary<string, object> eventData = new Dictionary<string, object> ();
//			eventData.Add (new KeyValuePair<string, object> ("Number", gameState.levelNumber)); todo
			eventData.Add (new KeyValuePair<string, object> ("Score", gameState.score));
			eventData.Add (new KeyValuePair<string, object> ("MaxScore", gameState.maxScore));
			eventData.Add (new KeyValuePair<string, object> ("MovesLeft", gameState.movesLeft));
			Analytics.CustomEvent ("GameExit", eventData);
		}
	}
}

