using System.Collections.Generic;
using System.Linq;

namespace Gibberish.AST
{
	internal class DefineThunkNode : Declaration
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

		public static Parse From(Block block)
		{
			var parseErrors = block.Prelude.Errors.Concat(block.Body.SelectMany(p=>p.Errors));
			var maybeName = block.Prelude.MaybeName;
			if (maybeName == null)
			{
				parseErrors = new[]
				{
					ParseError.MissingThunkName()
				}.Concat(parseErrors);
			}
			return Parse.Valid(new DefineThunkNode(maybeName.Name, block.Body.SelectMany(b => b.Statements)), parseErrors.ToList());
		}
	}
}
