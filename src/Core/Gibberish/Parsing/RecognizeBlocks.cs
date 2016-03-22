using System.Linq;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using IronMeta.Matcher;

namespace Gibberish.Parsing
{
	using _RecognizeBlocks_Item = MatchItem<char, Recognition>;

	partial class RecognizeBlocks
	{
		private static Recognition _ExtractStatementAndErrors(_RecognizeBlocks_Item content)
		{
			var rawContent = content.AsString();
			var coreContent = rawContent.TrimEnd();
			var errors = rawContent.Length != coreContent.Length
				? new[]
				{
					ParseError.IllegalWhitespaceAtEnd(rawContent.Substring(coreContent.Length))
				}
				: Recognition.NoErrors;
			return Recognition.With(new BareStatement(coreContent, errors.ToArray()));
		}
	}
}
