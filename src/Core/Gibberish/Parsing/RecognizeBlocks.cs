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
			var errors = rawContent.Length == coreContent.Length
				? Recognition.NoErrors
				: new[]
				{
					ParseError.IllegalWhitespaceAtEnd(rawContent.Substring(coreContent.Length))
				};
			return Recognition.With(new UnknownStatement(coreContent, errors.ToArray()));
		}

		private static Recognition _ExtractBlockAndErrors(_RecognizeBlocks_Item prelude, _RecognizeBlocks_Item body, _RecognizeBlocks_Item trailingWhitespace)
		{
			var extraAtEnd = trailingWhitespace.AsString();
			var preludeErrors = extraAtEnd.Length == 0
				? Recognition.NoErrors
				: new[]
				{
					ParseError.IllegalWhitespaceAtEnd(extraAtEnd)
				};
			var blockErrors = Recognition.NoErrors;
			return new UnknownBlock(new UnknownPrelude(prelude.AsString(), preludeErrors), body.Results.SelectMany(r => r.Items), blockErrors).AsRecognition();
		}
	}
}
