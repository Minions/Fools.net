using System.Collections.Generic;

namespace Gibberish.AST._1_Bare
{
	public class CommentDefinitionBlockPrelude : LanguageConstruct
	{
		public CommentDefinitionBlockPrelude(int commentId, IEnumerable<ParseError> errors) : base(errors)
		{
			CommentId = commentId;
		}

		public int CommentId { get; }
	}
}
