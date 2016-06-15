using System;

namespace Controller
{
	public class WaitBeforeNextLevel:Action
	{
		float _waitFor = 2.4f;

		public override PrefromResult Perform (float delta)
		{
			_waitFor -= delta;
			if (_waitFor < 0) 
				return PrefromResult.COMPLETED;
			else
				return PrefromResult.BLOCK;
		}
	}
}

