using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gibberish.Parsing;
using JetBrains.Annotations;

namespace Gibberish.AST
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

		public static ParseError IllegalWhitespaceAtEnd(string input)
		{
			return new ParseError(UiStrings.IllegalWhitespaceAtEnd, EscapeWhitespace(input));
		}

		private static string EscapeWhitespace(string input)
		{
			var replacements = new Dictionary<char, string>
			{
				{'\\', "\\\\"},
				{'\t', "\\t"},
				{'\n', "\\n"},
				{'\r', "\\r"},
				{'\v', "\\v"}
			};
			if (string.IsNullOrEmpty(input)) { return input; }
			var retval = new StringBuilder(input.Length);
			foreach (var ch in input)
			{
				string escapedWhitespace;
				if (replacements.TryGetValue(ch, out escapedWhitespace)) { retval.Append(escapedWhitespace); }
				else
				{ retval.Append(ch); }
			}
			return retval.ToString();
		}

		[NotNull] private static readonly string[] KnownLanguages = {
			"fasm",
			"fools"
		};
	}
}
