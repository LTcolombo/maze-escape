using System;

namespace Controller
{
	public class WaitBeforeNextLevel:Action
	{
		float _waitFor = 0.6f;

		public override PrefromResult Perform (float delta)
		{
			_waitFor -= delta;
			if (_waitFor < 0) 
				return PrefromResult.COMPLETED;
			else
				return PrefromResult.SUCCESS;
		}
	}
}

