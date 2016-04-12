using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class CommentBuilder : AstBuilder<LanguageConstruct>
	{
		public CommentBuilder(int commentId, [NotNull] string content)
		{
			CommentId = commentId;
			Content = content;
		}

		public int CommentId { get; }
		[NotNull]
		public string Content { get; }

		internal override void Build(List<LanguageConstruct> destination)
		{
			destination.Add(new CommentDefinition(CommentId, Content, Errors));
		}
	}
}
