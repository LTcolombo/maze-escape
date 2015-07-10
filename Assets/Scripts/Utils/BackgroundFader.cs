using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BackgroundFader : MonoBehaviour
{
	public Color startColor;

	public Color  endColor;

	public float time;

	// Use this for initialization
	void Start ()
	{
		Camera.main.backgroundColor = startColor;
		Camera.main.DOColor (endColor, time);
	}
}

