using System.Collections.Generic;
using System.Linq;

namespace Gibberish.AST
{
	internal class DefineThunkNode : ParseTree
	{
		public DefineThunkNode(string name, IEnumerable<Statement> body)
		{
			this.name = name;
			this.body = body.ToList();
		}

		public readonly List<Statement> body;

		public readonly string name;

		// ReSharper disable once UnusedMember.Global
		public readonly string type = "define.thunk";
	}
}
