using System.Collections.Generic;

namespace Gibberish.AST._1_Bare.Builders
{
	public class MultilineCommentStatementBuilder : AstBuilderSupportingErrors<LanguageConstruct>
	{
		public bool StartsParagraph { get; private set; }

		public string Content { get; }

		public MultilineCommentStatementBuilder(string content)
		{
			this.Content = content;
		}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			destination.Add(new MultilineCommentDefinitionStatement(1, Content, Errors));
		}

		public MultilineCommentStatementBuilder ThatStartsParagraph()
		{
			StartsParagraph = true;
			return this;
		}
	}
}