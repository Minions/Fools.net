using System.Collections.Generic;

namespace Gibberish.AST._1_Bare
{
	public class BlankLine : LanguageConstruct
	{
		public int IndentationDepth { get; }

		public BlankLine(int indentationDepth, IEnumerable<ParseError> errors) : base(errors)
		{
			IndentationDepth = indentationDepth;
		}
	}
}