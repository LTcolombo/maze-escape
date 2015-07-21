using UnityEngine;
using UnityEngine.UI;

public class MaxScoreMediator : MonoBehaviour {
	
	public string prefix = "MAX SCORE: ";
	public string format = "F0";
	
	Text _target;
	int _previousValue;
	
	void Awake(){
		_target = GetComponent<Text> ();
		_previousValue = 0;
	}
	
	// Update is called once per frame
	void OnGameStateUpdated (GameState state) {
		if (_previousValue == state.maxScore)
			return;
			
		_previousValue = state.maxScore;
		_target.text = prefix + state.maxScore.ToString (format);
	}
}
