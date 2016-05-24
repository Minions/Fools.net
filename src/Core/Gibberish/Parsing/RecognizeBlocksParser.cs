using Gibberish.AST._1_Bare;
using IronMeta.Matcher;

namespace Gibberish.Parsing
{
	partial class RecognizeBlocks
	{
		public MatchResult<char, LanguageConstruct> ParseWholeFile(string input)
		{
			var result = GetMatch(input, WholeFile);
			return result;
		}
	}
}
