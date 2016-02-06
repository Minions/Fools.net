using JetBrains.Annotations;

namespace Gibberish.Execution
{
	public class District
	{
		public District(City city, string name)
		{
			_city = city;
			_name = name;
		}

		[CanBeNull]
		public ThunkDescriptor Name(string name) { return null; }

		private readonly City _city;
		private readonly string _name;
	}
}
