namespace Controller
{
	public enum PrefromResult {
		SUCCESS,
		FAILURE,
		COMPLETED
	}
	
	abstract public class Action
	{
		abstract public PrefromResult Perform(float delta);
	}
}

