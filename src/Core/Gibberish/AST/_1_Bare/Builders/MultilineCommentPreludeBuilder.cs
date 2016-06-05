using System.Collections.Generic;

namespace Gibberish.AST._1_Bare.Builders
{
	public class MultilineCommentPreludeBuilder : AstBuilderSupportingErrors<LanguageConstruct>
	{
		public bool StartsParagraph { get; private set; }

		public int CommentId { get; }

		public MultilineCommentPreludeBuilder(int commentId)
		{
			CommentId = commentId;
		}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			destination.Add(new MultilineCommentDefinitionPrelude(PossiblySpecified<bool>.WithValue(StartsParagraph), CommentId, Errors));
		}

		public MultilineCommentPreludeBuilder ThatStartsParagraph()
		{
			StartsParagraph = true;
			return this;
		}
	}
}