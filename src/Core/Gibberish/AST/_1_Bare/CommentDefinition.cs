using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class CommentDefinition : LanguageConstruct
	{
		public CommentDefinition(PossiblySpecified<bool> startsParagraph, int commentId, [NotNull] string content, [NotNull] IEnumerable<ParseError> errors) : base(errors)
		{
			StartsParagraph = startsParagraph;
			CommentId = commentId;
			Content = content;
		}

		public PossiblySpecified<bool> StartsParagraph { get; set; }
		public int CommentId { get; }
		[NotNull]
		public string Content { get; }
	}
}
