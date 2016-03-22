using JetBrains.Annotations;

namespace Gibberish.AST
{
	public class NameNode
	{
		public NameNode([NotNull] string value)
		{
			Name = value;
		}

		[NotNull]
		public string Name { get; }
	}
}
