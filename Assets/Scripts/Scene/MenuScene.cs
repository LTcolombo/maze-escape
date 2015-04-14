using UnityEngine;
using System.Collections;

public class MenuScene : MonoBehaviour {
	// Use this for initialization
	void Start () {
		addText ("Maze Escape", 48, 0.7f);
		addText ("High Score: " + PlayerPrefs.GetInt("High Score").ToString(), 24, 0.6f);
		addText ("Tap somewhere\nto play", 18, 0.45f);
	}
	
	void addText(string text, int size, float yPos) {
		GameObject textfield = new GameObject();
		textfield.transform.parent = transform;
		textfield.AddComponent<GUIText>();
		textfield.GetComponent<GUIText>().alignment = TextAlignment.Center;
		textfield.GetComponent<GUIText>().fontSize = size;
		textfield.GetComponent<GUIText>().text = text;
		textfield.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;
		textfield.transform.position = new Vector3(0.5f,yPos,0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			Application.LoadLevel("GameScene");
		} 
	}
}
