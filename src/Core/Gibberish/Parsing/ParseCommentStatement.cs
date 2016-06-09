using System.Collections.Generic;
using System.Text;
using Gibberish.AST;
using Gibberish.AST._1_Bare;

namespace Gibberish.Parsing
{
	internal static class ParseCommentStatement
	{
		public static LanguageConstruct Interpret(int indentationDepth, string content)
		{
			if (indentationDepth > 1)
			{
				var c = new StringBuilder();
				c.Append('\t', indentationDepth - 1);
				c.Append(content);
				content = c.ToString();
				indentationDepth = 1;
			}
			var errors = new List<ParseError>();

			return new CommentDefinitionBlockStatement(indentationDepth, content, errors);
		}
	}
}
