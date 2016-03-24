using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Gibberish.Parsing
{
	partial class RecognizeBlocks
	{
		[NotNull]
		private LanguageConstruct _ExtractBlankLine(int indentationDepth, string illegalWhitespace)
		{
			var errors = new List<ParseError>();
			if (illegalWhitespace.Length > 0) { errors.Add(ParseError.IllegalWhitespaceOnBlankLine(illegalWhitespace)); }
			return new BlankLine(indentationDepth, errors);
		}

		[NotNull]
		private static UnknownStatement _ExtractStatementAndErrors(int indentationDepth, string content)
		{
			var coreContent = content.TrimEnd();
			var extraAtEnd = content.Substring(coreContent.Length);

			var errors = new List<ParseError>();
			var comments = new List<int>();
			var statement = _ExtractCommentsAndReturnEverythingBeforeThem(errors, coreContent, comments);
			_CheckForWhitespaceErrors(errors, statement, extraAtEnd);
			return new UnknownStatement(indentationDepth, statement, comments, errors);
		}

		[NotNull]
		private UnknownPrelude _ExtractPreludeAndErrors(int indentationDepth, string content, string contentAfterColon)
		{
			var extraAtEnd = contentAfterColon;
			var possibleComment = extraAtEnd.TrimEnd();
			extraAtEnd = extraAtEnd.Substring(possibleComment.Length);
			var preludeErrors = new List<ParseError>();
			var comments = new List<int>();
			_ExtractCommentsAndReturnEverythingBeforeThem(preludeErrors, possibleComment, comments);
			_CheckForWhitespaceErrors(preludeErrors, content, extraAtEnd);
			return new UnknownPrelude(indentationDepth, content, comments, preludeErrors);
		}

		[NotNull]
		private LanguageConstruct _ExtractSingleLineCommentDefinition(string commentId, string content, string commentSeparator)
		{
			var errors = new List<ParseError>();
			int commentNumber;
			if (!int.TryParse(commentId, out commentNumber)) {
				errors.Add(ParseError.MissingIdInCommentDefinition(content.Substring(0, 8)));
			}
			else if (!" ".Equals(commentSeparator)) { errors.Add(ParseError.IncorrectCommentDefinitionSeparator(commentSeparator)); }
			return new CommentDefinition(commentNumber, content, errors);
		}

		[NotNull]
		private LanguageConstruct _ExtractMultiLineCommentDefinition(string commentId, string content, string commentSeparator, string commentEnd)
		{
			var errors = new List<ParseError>();
			int commentNumber;
			if (!int.TryParse(commentId, out commentNumber)) {
				errors.Add(ParseError.MissingIdInCommentDefinition(content.Substring(0, 8)));
			}
			else if (!" ".Equals(commentSeparator)) { errors.Add(ParseError.IncorrectCommentDefinitionSeparator(commentSeparator)); }
			if (commentEnd.Length == 0) {
				errors.Add(ParseError.MultilineCommentWithoutEnd());
			}
			else if (!"\"\"\"".Equals(commentEnd)) { errors.Add(ParseError.ErrorAtEndOfMultilineComment(commentEnd)); }
			return new CommentDefinition(commentNumber, content, errors);
		}

		[NotNull]
		private static string _ExtractCommentsAndReturnEverythingBeforeThem([NotNull] List<ParseError> errors, [NotNull] string content, [NotNull] List<int> comments)
		{
			var statementAndComment = content.Split(
				new[]
				{
					'#'
				},
				2);
			var statement = statementAndComment[0];
			if (statementAndComment.Length > 1)
			{
				statement = statement.TrimEnd();
				var whitespaceBeforeComment = statementAndComment[0].Substring(statement.Length);
				if (!whitespaceBeforeComment.Equals("\t\t")) { errors.Add(ParseError.IncorrectCommentSeparator(whitespaceBeforeComment + "#")); }
				_ExtractCommmentRefs(errors, statementAndComment[1], comments);
			}
			return statement;
		}

		private static void _ExtractCommmentRefs(List<ParseError> errors, string remainingCommentToParse, List<int> comments)
		{
			var formatErrorFound = false;
			var references = remainingCommentToParse.Split(
				new[]
				{
					", "
				},
				StringSplitOptions.None);
			foreach (var match in references.Select(reference => CommentMatcher.Match(reference)))
			{
				if (!match.Success)
				{
					formatErrorFound = true;
					break;
				}
				comments.Add(int.Parse(match.Groups[1].Value));
			}
			if (formatErrorFound) { errors.Add(ParseError.IncorrectCommentFormat(remainingCommentToParse)); }
		}

		private static void _CheckForWhitespaceErrors(List<ParseError> errors, string coreContent, string extraAtEnd)
		{
			if (extraAtEnd.Length != 0) { errors.Add(ParseError.IllegalWhitespaceAtEnd(extraAtEnd)); }
			if (coreContent.Contains('\t')) { errors.Add(ParseError.IllegalTabInLine()); }
		}

		[NotNull] private static readonly Regex CommentMatcher = new Regex(@"^\[([0-9]+)\]$");
	}
}
