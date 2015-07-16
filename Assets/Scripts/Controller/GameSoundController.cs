using UnityEngine;
using System.Collections;

public class GameSoundController : MonoBehaviour
{

	private static int[,] SEQUENCE = new int[,] {
		{0, 0, 0, 1},
		{0, 0, 0, 0},
		{0, 0, 0, 0},
		{0, 0, 0, 0},
		{0, 1, 0, 0},
		{0, 0, 0, 0},
		{0, 0, 1, 0},
		{1, 0, 0, 0}
	};
	private int _index = 0;


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void PlayNext ()
	{
		_index++;
		if (_index >= SEQUENCE.GetLength(0)) 
			_index = 0;
			
		for (int i = 0; i < SEQUENCE.GetLength(1); i++) {
		
		}
	}
}
