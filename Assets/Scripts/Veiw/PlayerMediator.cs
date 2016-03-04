using UnityEngine;
using System.Collections;
using DG.Tweening;
using Model;
using Model.Data;
using Notifications;

namespace View
{
	public class PlayerMediator : MonoBehaviour
	{
		private SpriteRenderer _renderer;
		private AudioSource _audio;

		// Use this for initialization
		void Start ()
		{
			transform.eulerAngles = new Vector3 (0, 0, -90 * PlayerModel.Instance ().directionIdx);
			transform.localPosition = new Vector3 (LevelModel.NODE_SIZE * PlayerModel.Instance ().cellPosition.x, 
				LevelModel.NODE_SIZE * PlayerModel.Instance ().cellPosition.y, 
				0);
			                                      
			_renderer = GetComponent<SpriteRenderer> ();
			_audio = GetComponent<AudioSource> ();

			MazePaceNotifications.MAZE_RECREATED.Add (OnMazeDataUpdated);
			MazePaceNotifications.EXIT_REACHED.Add (OnExitReached);
			MazePaceNotifications.PLAYER_STUCK.Add (OnStuck);
		}

		void OnMazeDataUpdated (MazeModel data)
		{
			_renderer.enabled = true;
		}


		void OnExitReached ()
		{
			_renderer.enabled = false;
		}

		void OnStuck ()
		{
			_audio.Play ();
		}

		public void OnDestroy ()
		{
			MazePaceNotifications.MAZE_RECREATED.Remove (OnMazeDataUpdated);
			MazePaceNotifications.EXIT_REACHED.Remove (OnExitReached);
			MazePaceNotifications.PLAYER_STUCK.Remove (OnStuck);
		}
	}
}
