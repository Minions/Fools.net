using System.Collections.Generic;
using System.Linq;
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
			return new ParseError(UiStrings.IllegalWhitespaceAtEnd, input);
		}

		public static ParseError IncorrectCommentSeparator(string separatorUsed)
		{
			return new ParseError(UiStrings.IncorrectCommentSeparator, separatorUsed);
		}

		public static ParseError IncorrectCommentDefinitionSeparator(string separatorUsed)
		{
			return new ParseError(UiStrings.IncorrectCommentDefinitionSeparator, separatorUsed);
		}

		public static ParseError IncorrectCommentFormat(string comment)
		{
			return new ParseError(UiStrings.IncorrectCommentFormat, comment);
		}

		public static ParseError IllegalTabInLine()
		{
			return new ParseError(UiStrings.IllegalTabInLine);
		}

		public static ParseError MissingIdInCommentDefinition(string firstPartOfComment)
		{
			return new ParseError(UiStrings.MissingIdInCommentDefinition, firstPartOfComment);
		}

		public static ParseError IllegalWhitespaceOnBlankLine(string whitespace)
		{
			return new ParseError(UiStrings.IllegalWhitespaceOnBlankLine, whitespace);
		}

		public static ParseError ErrorAtEndOfMultilineComment(string endingFound)
		{
			return new ParseError(UiStrings.ErrorAtEndOfMultilineComment, endingFound);
		}

		public static ParseError MultilineCommentWithoutEnd()
		{
			return new ParseError(UiStrings.MultilineCommentWithoutEnd);
		}

		public static ParseError MissingNewlineAtEndOfFile()
		{
			return new ParseError(UiStrings.MissingNewlineAtEndOfFile);
		}

		public static ParseError IncorrectIndentation(int expectedIndentation, int actualIndentation)
		{
			return new ParseError(UiStrings.IncorrectIndentation, expectedIndentation, actualIndentation);
		}

		[NotNull] public static readonly IEnumerable<ParseError> NoErrors = Enumerable.Empty<ParseError>();

		[NotNull] private static readonly string[] KnownLanguages = {
			"fasm",
			"fools"
		};
	}
}
