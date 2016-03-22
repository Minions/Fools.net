using System.Collections.Generic;
using System.Linq;
using Gibberish.Parsing;
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

		[NotNull] public readonly string name;
		[NotNull] public readonly List<Statement> body;

		[NotNull]
		public static Parse From([NotNull] Block block)
		{
			var parseErrors = block.Prelude.Errors.Concat(block.Body.SelectMany(p => p.Errors));
			var maybeName = block.Prelude.MaybeName;
			if (maybeName == null)
			{
				parseErrors = new[]
				{
					ParseError.BlockWithMissingName("`define.thunk`")
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
