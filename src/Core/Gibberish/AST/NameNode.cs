using System.Collections.Generic;
using System.Linq;

namespace Gibberish.AST
{
	public class NameNode
	{
		public NameNode(IEnumerable<char> value) { Name = new string(value.ToArray()); }

		public string Name { get; }
	}
}
