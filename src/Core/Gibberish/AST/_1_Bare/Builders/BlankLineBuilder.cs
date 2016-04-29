using System.Collections.Generic;

namespace Gibberish.AST._1_Bare.Builders
{
	public class BlankLineBuilder : AstBuilderSupportingErrors<LanguageConstruct>
	{
		public BlankLineBuilder(PossiblySpecified<int> indentationDepth)
		{
			IndentationDepth = indentationDepth;
		}

		public PossiblySpecified<int> IndentationDepth { get; }

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			destination.Add(new BlankLine(IndentationDepth, Errors));
		}
	}
}
