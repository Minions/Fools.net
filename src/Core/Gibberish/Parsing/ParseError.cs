using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.Parsing
{
	public class ParseError
	{
		[StringFormatMethod("baseMesage")]
		private ParseError([NotNull] string baseMessage, [NotNull] params object[] messageParams)
		{
			message = string.Format(baseMessage, messageParams);
		}

		public static ParseError UnknownLanguage(IEnumerable<char> lang)
		{
			return new ParseError(UiStrings.UnknownLanguage, new string(lang.ToArray()), KnownLanguages.ToSetDisplayString());
		}

		[NotNull] public readonly string message;

		public static ParseError BlockWithMissingName(string block_type)
		{
			return new ParseError(UiStrings.MissingNameForBlock, block_type);
		}

		[NotNull] private static readonly string[] KnownLanguages = {
			"fasm", "fools"
		};
	}
}
