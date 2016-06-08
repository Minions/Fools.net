using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class CommentDefinitionBlockStatementBuilder : AstBuilderSupportingErrors<LanguageConstruct>
	{
		public CommentDefinitionBlockStatementBuilder([NotNull] string content)
		{
			Content = content;
		}

		[NotNull]
		public string Content { get; }

		public int IndentationDepth { get; private set; } = 1;

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			destination.Add(new CommentDefinitionBlockStatement(IndentationDepth, Content, Errors));
		}

		public CommentDefinitionBlockStatementBuilder WithIndentationDepth(int depth)
		{
			IndentationDepth = depth;
			return this;
		}
	}
}
