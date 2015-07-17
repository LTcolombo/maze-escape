using UnityEngine;
using UnityEngine.UI;

public class ScoreMediator : MonoBehaviour
{
	
	public string prefix = "SCORE: ";
	public string format = "F0";
	private Text _target;
	private int _previousValue;
	private bool _wasDescending;
	
	void Awake ()
	{
		_target = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void OnGameStateUpdated (GameState state)
	{
		if (_target == null) {
			Debug.Log ("ScoreController assigned to an object without UI.Text component");
			return;
		}
		
		bool isDescending = (state.score < _previousValue);
		
		if (isDescending != _wasDescending) {
			_wasDescending = isDescending;
			_target.color = _wasDescending ? new Color (0.8f, 0.2f, 0.2f) : new Color (0.56f, 0.56f, 0.56f);
		}
		
		_target.text = prefix + state.score.ToString (format);
		_previousValue = state.score;
	}
}
