using Controller;
using UnityEngine;
using System.Collections.Generic;

namespace View
{
	public class ActionInvoker<T>:MonoBehaviour
	{
		Queue<Action> _queue;
		ActionFactory<T> _factory;

		protected void CreateQueue(){
			_factory = new ActionFactory<T>();
			_queue = new Queue<Action> ();
		}

		protected void InvokeCommand (T commandId)
		{
			_queue.Enqueue (_factory.Create(commandId));
		}

		protected void InvokeActions()
		{
			int commandCount = _queue.Count; //retaining commands will enqueue again
			while (commandCount > 0) {
				Action action = _queue.Peek ();
				commandCount--;
				PrefromResult result = action.Perform (Time.deltaTime);

				switch (result) {
				case(PrefromResult.COMPLETED):
					_queue.Dequeue ();
					break;

				case(PrefromResult.BLOCK):
					return;

				case(PrefromResult.PROCEED):
					_queue.Dequeue ();
					_queue.Enqueue (action);
					break;
				}
			}
		}
	}
}

