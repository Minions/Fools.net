using System.Collections.Generic;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Gibberish.Parsing.LineParsers
{
	internal static class ParseBlankLines
	{
		public static bool Matches(string content)
		{
			return string.IsNullOrWhiteSpace(content);
		}

		[NotNull]
		public static LanguageConstruct Interpret(int indentationDepth, string illegalWhitespace)
		{
			var errors = new List<ParseError>();
			if (illegalWhitespace.Length > 0) { errors.Add(ParseError.IllegalWhitespaceOnBlankLine(illegalWhitespace)); }
			return new BlankLine(PossiblySpecified<int>.WithValue(indentationDepth), errors);
		}
	}
}
