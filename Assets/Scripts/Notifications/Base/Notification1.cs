using System.Collections.Generic;


namespace Notifications.Base {
	public class Notification1<T>
	{
		public delegate void Notification1Callback (T p);

		private List<Notification1Callback> _callbacks;
		
		public Notification1(){
			_callbacks = new List<Notification1Callback>();
		}
		
		public void Dispatch (T p)
		{
			for (var i = 0; i < _callbacks.Count; i++) {
				_callbacks [i].Invoke (p);
			}
		}

		// Use this for initialization
		public void Add (Notification1Callback callback)
		{
			_callbacks.Add (callback);
		}
		
		// Update is called once per frame
		public void Remove (Notification1Callback callback)
		{
			_callbacks.Remove (callback);
		}
	}
}

