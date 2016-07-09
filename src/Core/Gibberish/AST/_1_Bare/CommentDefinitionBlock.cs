using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class CommentDefinitionBlock : LanguageConstruct
	{
		public CommentDefinitionBlock(PossiblySpecified<bool> startsParagraph, CommentDefinitionBlockPrelude prelude, IEnumerable<CommentDefinitionBlockStatement> body, List<ParseError> errors)
			: base(errors)
		{
			StartsParagraph = startsParagraph;
			Prelude = prelude;
			Body = body.ToList();
		}

		public PossiblySpecified<bool> StartsParagraph { get; }
		public CommentDefinitionBlockPrelude Prelude { get; }
		[NotNull]
		public IEnumerable<CommentDefinitionBlockStatement> Body { get; }
	}
}
