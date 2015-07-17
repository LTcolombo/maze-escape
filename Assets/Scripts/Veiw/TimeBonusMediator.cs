using UnityEngine;
using UnityEngine.UI;

public class TimeBonusMediator : MonoBehaviour {
	
	public string prefix = "TIME BONUS: ";
	public string format = "F1";
	
	private Text _target;
	
	void Awake(){
		_target = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void OnGameStateUpdated (GameState state) {
		if (_target == null) {
			Debug.Log("TimeBonusController assigned to an object without UI.Text component");
			return;
		}
		
		_target.text = prefix + state.timeBonus.ToString (format);
	}
}
