using System.Collections.Generic;

namespace Notifications.Base {
	public class Notification0
	{
		public delegate void Notification0Callback ();
		
		private List<Notification0Callback> _callbacks;
		
		public Notification0(){
			_callbacks = new List<Notification0Callback>();
		}
		
		public void Dispatch ()
		{
			for (var i = 0; i < _callbacks.Count; i++) {
				_callbacks [i].Invoke ();
			}
		}
		
		// Use this for initialization
		public void Add (Notification0Callback callback)
		{
			_callbacks.Add (callback);
		}
		
		// Update is called once per frame
		public void Remove (Notification0Callback callback)
		{
			_callbacks.Remove (callback);
		}
	}
}

