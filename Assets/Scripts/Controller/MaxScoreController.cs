using UnityEngine;
using UnityEngine.UI;

public class MaxScoreController : MonoBehaviour {
	
	public string prefix = "MAX SCORE: ";
	public string format = "F0";
	
	private Text _target;
	
	void Awake(){
		_target = GetComponent<Text> ();
		Debug.Log(_target);
	}
	
	// Update is called once per frame
	void OnGameStateUpdated (GameState state) {
		if (_target == null) {
			Debug.Log("MaxScoreController assigned to an object without UI.Text component");
			return;
		}
		_target.text = prefix + state.maxScore.ToString (format);
	}
}
