namespace Controller
{
	public enum PrefromResult {
		PROCEED,
		BLOCK,
		COMPLETED
	}
	
	abstract public class Action
	{
		abstract public PrefromResult Perform(float delta);
	}
}

