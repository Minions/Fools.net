using System.Collections.Generic;

namespace Gibberish.AST._1_Bare.Builders
{
	public class CommentDefinitionBlockStatementBuilder : AstBuilderSupportingErrors<LanguageConstruct>
	{
		public string Content { get; }

		public CommentDefinitionBlockStatementBuilder(string content)
		{
			this.Content = content;
		}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			destination.Add(new CommentDefinitionBlockStatement(1, Content, Errors));
		}
	}
}