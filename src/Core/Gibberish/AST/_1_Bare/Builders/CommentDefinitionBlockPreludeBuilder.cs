using System.Collections.Generic;

namespace Gibberish.AST._1_Bare.Builders
{
	public class CommentDefinitionBlockPreludeBuilder : AstBuilderSupportingErrors<LanguageConstruct>
	{
		public CommentDefinitionBlockPreludeBuilder(int commentId)
		{
			CommentId = commentId;
		}

		public int CommentId { get; }

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			destination.Add(new CommentDefinitionBlockPrelude(CommentId, Errors));
		}
	}
}
