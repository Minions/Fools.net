using System.Collections.Generic;

namespace Gibberish.AST._1_Bare.Builders
{
	public class CommentDefinitionBlockPreludeBuilder : AstBuilderSupportingErrors<LanguageConstruct>
	{
		public int CommentId { get; }

		public CommentDefinitionBlockPreludeBuilder(int commentId)
		{
			CommentId = commentId;
		}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			destination.Add(new CommentDefinitionBlockPrelude(PossiblySpecified<bool>.Unspecifed, CommentId, Errors));
		}
	}
}