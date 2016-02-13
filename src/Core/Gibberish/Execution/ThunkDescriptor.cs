using JetBrains.Annotations;

namespace Gibberish.Execution
{
	public class ThunkDescriptor
	{
		public ThunkDescriptor([NotNull] string name) { this.name = name; }

		[NotNull]
		public string name { get; private set; }
	}
}
