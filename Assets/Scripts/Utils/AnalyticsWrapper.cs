using Model;
using System.Collections.Generic;
using UnityEngine.Analytics;

namespace Utils
{
	public class AnalyticsWrapper
	{
		public static void ReportGameLost (GameModel game)
		{
			IDictionary<string, object> eventData = new Dictionary<string, object> ();
//			eventData.Add (new KeyValuePair<string, object> ("Number", gameState.levelNumber)); todo
			eventData.Add (new KeyValuePair<string, object> ("Score", game.score));
			eventData.Add (new KeyValuePair<string, object> ("MaxScore", game.maxScore));
			eventData.Add (new KeyValuePair<string, object> ("MovesLeft", game.movesLeft));
			
			Analytics.CustomEvent ("GameLost", eventData);
		}
		
		public static void ReportGamePaused (GameModel game)
		{
			IDictionary<string, object> eventData = new Dictionary<string, object> ();
//			eventData.Add (new KeyValuePair<string, object> ("Number", gameState.levelNumber)); todo
			eventData.Add (new KeyValuePair<string, object> ("Score", game.score));
			eventData.Add (new KeyValuePair<string, object> ("MaxScore", game.maxScore));
			eventData.Add (new KeyValuePair<string, object> ("MovesLeft", game.movesLeft));
			Analytics.CustomEvent ("GamePaused", eventData);
		}
		
		public static void ReportReturnToMenu (GameModel game)
		{
			IDictionary<string, object> eventData = new Dictionary<string, object> ();
//			eventData.Add (new KeyValuePair<string, object> ("Number", gameState.levelNumber)); todo
			eventData.Add (new KeyValuePair<string, object> ("Score", game.score));
			eventData.Add (new KeyValuePair<string, object> ("MaxScore", game.maxScore));
			eventData.Add (new KeyValuePair<string, object> ("MovesLeft", game.movesLeft));
			Analytics.CustomEvent ("GameExit", eventData);
		}
	}
}

