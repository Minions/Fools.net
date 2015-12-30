using System.Collections.Generic;
using System.Linq;

namespace Gibberish.AST
{
	internal class NameNode : ParseTree
	{
		public NameNode(IEnumerable<char> value) { Name = new string(value.ToArray()); }

		public string Name { get; }
	}
}
