using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	internal class DefineThunkNode : Declaration
	{
		private DefineThunkNode(string name, IEnumerable<Statement> body)
		{
			this.name = name;
			this.body = body.ToList();
		}

		public readonly List<Statement> body;

		public readonly string name;

		public readonly string type = "define.thunk";

		[NotNull]
		public static Parse From([NotNull] Block block)
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

		[NotNull]
		public static Parse From([NotNull] string name, [NotNull] IEnumerable<Statement> body)
		{
			return Parse.Valid(new DefineThunkNode(name, body), Parse.NoErrors);
		}
	}
}
