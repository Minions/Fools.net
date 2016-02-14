using System.Collections.Generic;
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
		public ThunkDescriptor Name([NotNull] string name)
		{
			ThunkDescriptor result;
			_thunks.TryGetValue(name, out result);
			return result;
		}

		private readonly City _city;
		private readonly string _name;
		[NotNull] private readonly Dictionary<string, ThunkDescriptor> _thunks = new Dictionary<string, ThunkDescriptor>();

		public void define_name(ThunkDescriptor thunk) { _thunks.Add(thunk.name, thunk); }
	}
}
