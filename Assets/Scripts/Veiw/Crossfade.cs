using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace View {
	[AddComponentMenu ("UI/Effects/SceneCrossfade")]
	public class Crossfade : MonoBehaviour
	{
		public Color startColor;
		public Color endColor;

		public float time;

		// Use this for initialization
		void Start ()
		{
			Camera.main.backgroundColor = startColor;
			Camera.main.DOColor (endColor, time);
		}
	}
}
