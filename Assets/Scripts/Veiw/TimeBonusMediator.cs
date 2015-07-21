using UnityEngine;
using UnityEngine.UI;

public class TimeBonusMediator : MonoBehaviour {
	
	public string prefix = "TIME BONUS: ";
	public string format = "F1";
	
	Text _target;
	float _previousValue;
	
	void Awake(){
		_target = GetComponent<Text> ();
		_previousValue = 0;
	}
	
	// Update is called once per frame
	void OnGameStateUpdated (GameState state) {
		
		if (_previousValue == state.timeBonus)
			return;
			
		_previousValue = state.timeBonus;
		_target.text = prefix + state.timeBonus.ToString (format);
	}
}
