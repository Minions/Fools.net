using System.Collections.Generic;
using System.Linq;
using Gibberish.Parsing;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	internal class SpecificationSection : Declaration
	{
		[NotNull] public readonly IEnumerable<string> headers;
		[NotNull] public readonly IEnumerable<Declaration> paragraphs;

		public SpecificationSection([NotNull] IEnumerable<string> headers, [NotNull] IEnumerable<Declaration> paragraphs)
		{
			this.headers = headers;
			this.paragraphs = paragraphs;
		}

		[NotNull]
		public static Parse From([NotNull] Block block)
		{
			var parseErrors = block.Prelude.Errors.Concat(block.Body.SelectMany(p => p.Errors));
			var headers = block.Prelude.Texts.ToList();
			if (!headers.Any())
			{
				parseErrors = new[]
				{
					ParseError.BlockWithMissingName("`specify`")
				}.Concat(parseErrors);
			}
			return Parse.Valid(new SpecificationSection(headers, block.Body.SelectMany(b => b.Declarations)), parseErrors.ToList());
		}
	}
}