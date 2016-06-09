using System.Collections.Generic;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Gibberish.Parsing
{
	internal static class ParsePreludes {
		public static LanguageConstruct Interpret(int indentationDepth, string content)
		{
			var parts = content.Split(
				new[]
				{
					':'
				},
				2);
			return _ExtractPreludeAndErrors(indentationDepth, parts[0], parts[1]);
		}

		public static bool Matches(string content)
		{
			return content.Contains(":");
		}

		[NotNull]
		private static UnknownPrelude _ExtractPreludeAndErrors(int indentationDepth, string content, string contentAfterColon)
		{
			var extraAtEnd = contentAfterColon;
			var possibleComment = extraAtEnd.TrimEnd();
			extraAtEnd = extraAtEnd.Substring(possibleComment.Length);
			var errors = new List<ParseError>();
			var comments = new List<int>();
			ParseCommentReferences.ExtractCommentsAndReturnEverythingBeforeThem(errors, possibleComment, comments);
			ParseCommentReferences.CheckForWhitespaceErrors(errors, content, extraAtEnd);
			return new UnknownPrelude(PossiblySpecified<int>.WithValue(indentationDepth), content, comments, errors);
		}
	}
}