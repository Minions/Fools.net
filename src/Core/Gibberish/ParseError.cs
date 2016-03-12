using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish
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

		public string message;

		public static ParseError MissingThunkName()
		{
			return new ParseError(UiStrings.MissingDefineThunkName);
		}

		[NotNull] private static readonly string[] KnownLanguages = {
			"fasm"
		};
	}
}
