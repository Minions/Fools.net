using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class CommentDefinition : LanguageConstruct
	{
		public CommentDefinition(int commentId, [NotNull] string content, [NotNull] IEnumerable<ParseError> errors) : base(errors)
		{
			CommentId = commentId;
			Content = content;
		}

		public int CommentId { get; }
		[NotNull]
		public string Content { get; }
	}
}