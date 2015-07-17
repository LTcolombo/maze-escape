using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
	
	public string prefix = "SCORE: ";
	public string format = "F0";
	
	private Text _target;
	
	void Start(){
		_target = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void OnGameStateUpdated (GameState state) {
		if (_target == null) {
			Debug.Log("ScoreController assigned to an object without UI.Text component");
			return;
		}
		
		_target.text = prefix + state.score.ToString (format);
	}
}
