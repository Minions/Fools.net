using System.Collections.Generic;
using System.Linq;
using Gibberish.AST;
using Gibberish.AST._1_Bare;

namespace Gibberish.Parsing
{
	public class AssembleBlocks
	{
		public List<LanguageConstruct> Transform(List<LanguageConstruct> source)
		{
			return new List<LanguageConstruct>
			{
				new UnknownBlock((UnknownPrelude) source.First(), source.Skip(1), ParseError.NoErrors)
			};
		}
	}
}
