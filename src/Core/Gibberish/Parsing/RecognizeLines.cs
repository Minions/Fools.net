using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Gibberish.Parsing
{
	public class RecognizeLines
	{
		[NotNull]
		public IEnumerable<LanguageConstruct> ParseWholeFile([NotNull] string input)
		{
			var hasNewlineatEndOfFile = false;
			if (input.EndsWith(CRLF))
			{
				hasNewlineatEndOfFile = true;
				input = input.Substring(0, input.Length - 2);
			}
			else if (input.EndsWith(LF) || input.EndsWith(CR))
			{
				hasNewlineatEndOfFile = true;
				input = input.Substring(0, input.Length - 1);
			}
			var result = input.Split(
				new[]
				{
					CRLF,
					LF,
					CR
				},
				StringSplitOptions.None)
				.Select(_InterpretLine)
				.Where(_ => _ != null)
				.ToList();
			if (!hasNewlineatEndOfFile) { result[result.Count - 1].Errors.Add(ParseError.MissingNewlineAtEndOfFile()); }
			return result;
		}

		[NotNull]
		private LanguageConstruct _InterpretLine([NotNull] string line)
		{
			var content = line.TrimStart('\t');
			var indentationDepth = line.Length - content.Length;
			if (content.StartsWith("##"))
			{
				inCommentSection = true;
				var match = _commentDefinitionBlockPreludePattern.Match(content);
				if (!match.Success) { return _ExtractCommentDefinitionBlockPrelude(indentationDepth, "", ""); }
				var commentId = match.Groups["commentId"].Value;
				var extra = match.Groups["extra"].Value;
				return _ExtractCommentDefinitionBlockPrelude(indentationDepth, commentId, extra);
			}
			if (content.StartsWith("#"))
			{
				inCommentSection = true;
				var match = _commentDefinitionPattern.Match(content);
				if (!match.Success)
				{
					return _ExtractSingleLineCommentDefinition(
						"",
						content.Substring(1)
							.TrimStart(),
						"",
						CRLF);
				}
				var commentId = match.Groups["commentId"].Value;
				var commentSeparator = match.Groups["commentSeparator"].Value;
				var firstLineContent = match.Groups["firstLineContent"].Value;

				return _ExtractSingleLineCommentDefinition(commentId, firstLineContent, commentSeparator, CRLF);
			}
			if (inCommentSection) { return _ExtractMultiLineCommentStatement(indentationDepth, content); }
			if (string.IsNullOrWhiteSpace(content)) { return _ExtractBlankLine(indentationDepth, content, CRLF); }

			if (content.Contains(":"))
			{
				var parts = content.Split(
					new[]
					{
						':'
					},
					2);
				return _ExtractPreludeAndErrors(indentationDepth, parts[0], parts[1], CRLF);
			}
			return _ExtractStatementAndErrors(indentationDepth, content, CRLF);
		}

		[NotNull]
		private LanguageConstruct _ExtractBlankLine(int indentationDepth, string illegalWhitespace, string newline)
		{
			var errors = new List<ParseError>();
			_RequireNewline(newline, errors);
			if (illegalWhitespace.Length > 0) { errors.Add(ParseError.IllegalWhitespaceOnBlankLine(illegalWhitespace)); }
			return new BlankLine(PossiblySpecified<int>.WithValue(indentationDepth), errors);
		}

		[NotNull]
		private static UnknownStatement _ExtractStatementAndErrors(int indentationDepth, string content, string newline)
		{
			var coreContent = content.TrimEnd();
			var extraAtEnd = content.Substring(coreContent.Length);

			var errors = new List<ParseError>();
			var comments = new List<int>();
			_RequireNewline(newline, errors);
			var statement = _ExtractCommentsAndReturnEverythingBeforeThem(errors, coreContent, comments);
			_CheckForWhitespaceErrors(errors, statement, extraAtEnd);
			return new UnknownStatement(PossiblySpecified<bool>.Unspecifed, PossiblySpecified<int>.WithValue(indentationDepth), statement, comments, errors);
		}

		[NotNull]
		private UnknownPrelude _ExtractPreludeAndErrors(int indentationDepth, string content, string contentAfterColon, string newline)
		{
			var extraAtEnd = contentAfterColon;
			var possibleComment = extraAtEnd.TrimEnd();
			extraAtEnd = extraAtEnd.Substring(possibleComment.Length);
			var errors = new List<ParseError>();
			var comments = new List<int>();
			_RequireNewline(newline, errors);
			_ExtractCommentsAndReturnEverythingBeforeThem(errors, possibleComment, comments);
			_CheckForWhitespaceErrors(errors, content, extraAtEnd);
			return new UnknownPrelude(PossiblySpecified<int>.WithValue(indentationDepth), content, comments, errors);
		}

		[NotNull]
		private LanguageConstruct _ExtractSingleLineCommentDefinition(string commentId, string content, string commentSeparator, string newline)
		{
			var errors = new List<ParseError>();
			_RequireNewline(newline, errors);
			var commentNumber = _ExtractCommentNumber(commentId, content, commentSeparator, errors);
			return new CommentDefinition(PossiblySpecified<bool>.Unspecifed, commentNumber, content, errors);
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

		private static void _RequireNewline(string newline, List<ParseError> errors)
		{
			if (newline.Length == 0) { errors.Add(ParseError.MissingNewlineAtEndOfFile()); }
		}

		private static int _ExtractCommentNumber(string commentId, string content, string commentSeparator, List<ParseError> errors)
		{
			int commentNumber;
			if (!int.TryParse(commentId, out commentNumber)) {
				errors.Add(ParseError.MissingIdInCommentDefinition(content.Substring(0, 8)));
			}
			else if (!" ".Equals(commentSeparator)) { errors.Add(ParseError.IncorrectCommentDefinitionSeparator(commentSeparator)); }
			return commentNumber;
		}

		private LanguageConstruct _ExtractCommentDefinitionBlockPrelude(int indentationDepth, string commentId, string extra)
		{
			var errors = new List<ParseError>();

			if (!string.IsNullOrEmpty(extra)) { errors.Add(ParseError.IllegalContentAfterColonInPrelude(extra)); }

			int commentNumber;
			if (!int.TryParse(commentId, out commentNumber)) { errors.Add(ParseError.MissingIdInCommentDefinition(commentId.Substring(0, 8))); }

			return new CommentDefinitionBlockPrelude(commentNumber, errors);
		}

		private LanguageConstruct _ExtractMultiLineCommentStatement(int indentationDepth, string content)
		{
			if (indentationDepth > 1)
			{
				var c = new StringBuilder();
				c.Append('\t', indentationDepth - 1);
				c.Append(content);
				content = c.ToString();
				indentationDepth = 1;
			}
			var errors = new List<ParseError>();

			return new CommentDefinitionBlockStatement(indentationDepth, content, errors);
		}

		private const string CRLF = CR + LF;
		private const string CR = "\r";
		private const string LF = "\n";

		[NotNull] private static readonly Regex CommentMatcher = new Regex(@"^\[([0-9]+)\]$");

		[NotNull] private readonly Regex _commentDefinitionBlockPreludePattern = new Regex(@"(?x)
				^\#\#
					\[(?<commentId>[0-9]+)\]\:(?<extra>.*)
", RegexOptions.Compiled);

		[NotNull] private readonly Regex _commentDefinitionPattern = new Regex(@"(?x)^\#
				\[(?<commentId>[0-9]+)\]\:
				(?<commentSeparator>\s+)
				(?<firstLineContent>.*)
", RegexOptions.Compiled);

		bool inCommentSection;
	}
}
