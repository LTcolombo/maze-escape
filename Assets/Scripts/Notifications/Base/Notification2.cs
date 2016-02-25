using System.Collections.Generic;

namespace Notifications.Base {
	public class Notification2<T1, T2>
	{
		public delegate void Notification2Callback (T1 p1, T2 p2);
		
		private List<Notification2Callback> _callbacks;
		
		public Notification2(){
			_callbacks = new List<Notification2Callback>();
		}
		
		public void Dispatch (T1 p1, T2 p2)
		{
			for (var i = 0; i < _callbacks.Count; i++) {
				_callbacks [i].Invoke (p1, p2);
			}
		}
		
		// Use this for initialization
		public void Add (Notification2Callback callback)
		{
			_callbacks.Add (callback);
		}
		
		// Update is called once per frame
		public void Remove (Notification2Callback callback)
		{
			_callbacks.Remove (callback);
		}
	}
}