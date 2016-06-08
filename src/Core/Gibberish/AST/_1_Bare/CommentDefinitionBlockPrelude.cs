using System.Collections.Generic;

namespace Gibberish.AST._1_Bare
{
	public class CommentDefinitionBlockPrelude : LanguageConstruct
	{
		public CommentDefinitionBlockPrelude(PossiblySpecified<bool> startsParagraph, int commentId, List<ParseError> errors) : base(errors)
		{
			StartsParagraph = startsParagraph;
			CommentId = commentId;
		}

		public PossiblySpecified<bool> StartsParagraph { get; set; }
		public int CommentId { get; }
	}
}
