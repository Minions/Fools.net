using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class CommentDefinitionBlockStatement : LanguageConstruct
	{
		public CommentDefinitionBlockStatement(int indentationDepth, string content, IEnumerable<ParseError> errors) : base(errors)
		{
			IndentationDepth = indentationDepth;
			Content = content;
		}

		public int IndentationDepth { get; }
		[NotNull]
		public string Content { get; }
	}
}
