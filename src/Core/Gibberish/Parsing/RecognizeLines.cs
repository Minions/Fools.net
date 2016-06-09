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
			var hasNewlineatEndOfFile = DetectAndHandleNewlineAtFileEnd(ref input);
			var result = input.Split(
				new[]
				{
					CRLF,
					LF,
					CR
				},
				StringSplitOptions.None)
				.Select(_InterpretLine)
				.ToList();
			if (!hasNewlineatEndOfFile) { result[result.Count - 1].Errors.Add(ParseError.MissingNewlineAtEndOfFile()); }
			return result;
		}

		public void RememberThatHaveFoundCommentSection()
		{
			InCommentSection = true;
		}

		private const string CRLF = CR + LF;
		public bool InCommentSection { get; private set; }

		private static bool DetectAndHandleNewlineAtFileEnd([NotNull] ref string input)
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
			return hasNewlineatEndOfFile;
		}

		[NotNull]
		private LanguageConstruct _InterpretLine([NotNull] string line)
		{
			var content = line.TrimStart('\t');
			var indentationDepth = line.Length - content.Length;
			if (ParseCommentDefinitionPreludes.IsCommentDefinitionBlock(content)) { return ParseCommentDefinitionPreludes.InterpretCommentDefinitionBlock(this, content, indentationDepth); }
			if (ParseCommentDefinitions.IsCommentDefinition(content)) { return ParseCommentDefinitions.InterpretCommentDefinition(this, content); }
			if (InCommentSection) { return _ExtractMultiLineCommentStatement(indentationDepth, content); }
			if (string.IsNullOrWhiteSpace(content)) { return _ExtractBlankLine(indentationDepth, content); }

			if (content.Contains(":"))
			{
				var parts = content.Split(
					new[]
					{
						':'
					},
					2);
				return _ExtractPreludeAndErrors(indentationDepth, parts[0], parts[1]);
			}
			return _ExtractStatementAndErrors(indentationDepth, content);
		}

		[NotNull]
		private LanguageConstruct _ExtractBlankLine(int indentationDepth, string illegalWhitespace)
		{
			var errors = new List<ParseError>();
			if (illegalWhitespace.Length > 0) { errors.Add(ParseError.IllegalWhitespaceOnBlankLine(illegalWhitespace)); }
			return new BlankLine(PossiblySpecified<int>.WithValue(indentationDepth), errors);
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
			return new UnknownStatement(PossiblySpecified<bool>.Unspecifed, PossiblySpecified<int>.WithValue(indentationDepth), statement, comments, errors);
		}

		[NotNull]
		private UnknownPrelude _ExtractPreludeAndErrors(int indentationDepth, string content, string contentAfterColon)
		{
			var extraAtEnd = contentAfterColon;
			var possibleComment = extraAtEnd.TrimEnd();
			extraAtEnd = extraAtEnd.Substring(possibleComment.Length);
			var errors = new List<ParseError>();
			var comments = new List<int>();
			_ExtractCommentsAndReturnEverythingBeforeThem(errors, possibleComment, comments);
			_CheckForWhitespaceErrors(errors, content, extraAtEnd);
			return new UnknownPrelude(PossiblySpecified<int>.WithValue(indentationDepth), content, comments, errors);
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

		private const string CR = "\r";
		private const string LF = "\n";

		[NotNull] private static readonly Regex CommentMatcher = new Regex(@"^\[([0-9]+)\]$");
	}
}
