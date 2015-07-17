using UnityEngine;
using UnityEngine.UI;

public class MovesMediator : MonoBehaviour {
	
	public string prefix = "MOVES: ";
	public string format = "F0";
	
	private Text _target;
	
	void Awake(){
		_target = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void OnGameStateUpdated (GameState state) {
		if (_target == null) {
			Debug.Log("MovesController assigned to an object without UI.Text component");
			return;
		}
		
		_target.text = prefix + state.movesLeft.ToString (format);
	}
}
