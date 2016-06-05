using System.Collections.Generic;

namespace Gibberish.AST._1_Bare
{
	public class MultilineCommentDefinitionStatement : LanguageConstruct
	{
		public int IndentationDepth { get; }
		public string Content { get; }

		public MultilineCommentDefinitionStatement(int indentationDepth, string content, List<ParseError> errors) : base(errors)
		{
			IndentationDepth = indentationDepth;
			Content = content;
		}
	}
}