using System.Collections.Generic;
using System.Linq;
using Gibberish.Parsing;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	internal class Namespace : Declaration
	{
		public Namespace([NotNull] string name, IEnumerable<Declaration> declarations)
		{
			this.name = name;
			this.declarations = declarations;
		}

		[NotNull] public readonly string name;
		[NotNull] public readonly IEnumerable<Declaration> declarations;

		[NotNull]
		public static Parse From([NotNull] Block block)
		{
			var parseErrors = block.Prelude.Errors.Concat(block.Body.SelectMany(p => p.Errors));
			var maybeName = block.Prelude.MaybeName;
			if (maybeName == null)
			{
				parseErrors = new[]
				{
					ParseError.BlockWithMissingName("`in.namespace`")
				}.Concat(parseErrors);
			}
			return Parse.Valid(new Namespace(maybeName?.Name ?? string.Empty, block.Body.SelectMany(b => b.Declarations)), parseErrors.ToList());
		}
	}
}
