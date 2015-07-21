using UnityEngine;
using UnityEngine.UI;

public class MovesMediator : MonoBehaviour
{
	
	public string prefix = "MOVES: ";
	public string format = "F0";
	Text _target;
	AudioSource _audio;
	uint _previousValue;

	const int CRITICAL_MOVES = 5;
	
	void Awake ()
	{
		_target = GetComponent<Text> ();
		_audio = GetComponent<AudioSource> ();
		_previousValue = 0;
	}
	
	// Update is called once per frame
	void OnGameStateUpdated (GameState state)
	{
		if (state.movesLeft == _previousValue)
			return;
		
		if (!_audio.isPlaying)
			_audio.Play ();
			
		_previousValue = state.movesLeft;
			
		
		if (_previousValue < CRITICAL_MOVES && state.state != GameState.STATE_ENDED)
			_target.color = new Color (0.8f, 0.2f, 0.2f);
		else
			_target.color = new Color (0.56f, 0.56f, 0.56f);
			
		_target.text = prefix + state.movesLeft.ToString (format);
	}
}
