using Controller;
using UnityEngine;
using System.Collections.Generic;

namespace View
{
	public class ActionInvoker<T>:MonoBehaviour
	{
		Queue<Action> _queue;
		ActionFactory<T> _factory;

		virtual protected void Start(){
			_factory = new ActionFactory<T>();
			_queue = new Queue<Action> ();
		}

		protected void InvokeCommand (T commandId)
		{
			_queue.Enqueue (_factory.Create(commandId));
		}

		virtual protected void Update ()
		{
			int commandCount = _queue.Count; //retaining commands will enqueue again
			while (commandCount > 0) {
				Action action = _queue.Dequeue ();
				commandCount--;
				PrefromResult result = action.Perform (Time.deltaTime);

				switch (result) {
				case(PrefromResult.COMPLETED):
					break;

				case(PrefromResult.FAILURE):
					break;

				case(PrefromResult.SUCCESS):
					_queue.Enqueue (action);
					break;
				}
			}
		}
	}
}

