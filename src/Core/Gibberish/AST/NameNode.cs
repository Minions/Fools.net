using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	public class NameNode
	{
		public NameNode([NotNull] IEnumerable<char> value) { Name = new string(value.ToArray()); }

		[NotNull]
		public string Name { get; }
	}
}
