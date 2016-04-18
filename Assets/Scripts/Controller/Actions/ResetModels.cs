using Model;
using Notifications;

namespace Controller
{
	public class ResetModels:Action
	{
		override public PrefromResult Perform (float delta)
		{
			PlayerModel.Instance ().cellPosition.x = 0;
			PlayerModel.Instance ().cellPosition.y = 0;

			return PrefromResult.COMPLETED;
		}
	}
}

