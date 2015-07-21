using UnityEngine;
using UnityEngine.UI;

public class ScoreMediator : MonoBehaviour
{
	
	public string prefix = "SCORE: ";
	public string format = "F0";
	private Text _target;
	private int _previousState; //move to state?
	private int _previousValue; 
	private AudioSource _audio;
	
	void Awake ()
	{
		_target = GetComponent<Text> ();
		_audio = GetComponent<AudioSource> ();
		_previousValue = 0;
	}
	
	// Update is called once per frame
	void OnGameStateUpdated (GameState state)
	{
		if (_previousValue == state.score)
			return;
		
		if (state.state != _previousState) {
			if (state.state == GameState.STATE_STUCK)
				_target.color = new Color (0.8f, 0.2f, 0.2f);
			else if (state.state == GameState.STATE_ENDED)
				_target.color = new Color (0.2f, 0.8f, 0.2f);
			else
				_target.color = new Color (0.56f, 0.56f, 0.56f);
				
			_previousState = state.state;
		}
		
		_target.text = prefix + state.score.ToString (format);
		
		if (_previousValue < state.score && _audio!=null && !_audio.isPlaying)
			_audio.Play();
		
		_previousValue = state.score;
	}
}
