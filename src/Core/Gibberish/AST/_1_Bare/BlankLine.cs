using System.Collections.Generic;

namespace Gibberish.AST._1_Bare
{
	public class BlankLine : LanguageConstruct
	{
		public PossiblySpecified<int> IndentationDepth { get; }

		public BlankLine(PossiblySpecified<int> indentationDepth, IEnumerable<ParseError> errors) : base(errors)
		{
			IndentationDepth = indentationDepth;
		}
	}
}