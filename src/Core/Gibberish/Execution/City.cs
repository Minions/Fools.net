namespace Gibberish.Execution
{
	public class City
	{
		public District District(string name) { return new District(this, name);}
	}
}