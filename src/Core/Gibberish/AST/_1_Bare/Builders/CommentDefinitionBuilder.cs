using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class CommentDefinitionBuilder : AstBuilderSupportingErrors<LanguageConstruct>
	{
		public CommentDefinitionBuilder(int commentId, [NotNull] string content)
		{
			CommentId = commentId;
			Content = content;
		}

		public bool StartsParagraph { get; private set; }
		public int CommentId { get; }
		[NotNull]
		public string Content { get; }

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			destination.Add(new CommentDefinition(PossiblySpecified<bool>.WithValue(StartsParagraph), CommentId, Content, Errors));
		}

		public CommentDefinitionBuilder ThatStartsParagraph()
		{
			StartsParagraph = true;
			return this;
		}
	}
}
