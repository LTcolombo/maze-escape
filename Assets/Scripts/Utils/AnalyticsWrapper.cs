
using System.Collections.Generic;
using UnityEngine.Analytics;

namespace AssemblyCSharp
{
	public class AnalyticsWrapper
	{
		public static void ReportGameLost (GameState gameState)
		{
			IDictionary<string, object> eventData = new Dictionary<string, object> ();
			eventData.Add (new KeyValuePair<string, object> ("Number", gameState.levelNumber));
			eventData.Add (new KeyValuePair<string, object> ("Score", gameState.score));
			eventData.Add (new KeyValuePair<string, object> ("MaxScore", gameState.maxScore));
			eventData.Add (new KeyValuePair<string, object> ("MovesLeft", gameState.movesLeft));
			Analytics.CustomEvent ("GameLost", eventData);
		}
		
		public static void ReportGamePaused (GameState gameState)
		{
			IDictionary<string, object> eventData = new Dictionary<string, object> ();
			eventData.Add (new KeyValuePair<string, object> ("Number", gameState.levelNumber));
			eventData.Add (new KeyValuePair<string, object> ("Score", gameState.score));
			eventData.Add (new KeyValuePair<string, object> ("MaxScore", gameState.maxScore));
			eventData.Add (new KeyValuePair<string, object> ("MovesLeft", gameState.movesLeft));
			Analytics.CustomEvent ("GamePaused", eventData);
		}
	}
}

