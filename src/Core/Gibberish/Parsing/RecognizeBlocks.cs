using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using IronMeta.Matcher;
using JetBrains.Annotations;

namespace Gibberish.Parsing
{
	using _RecognizeBlocks_Item = MatchItem<char, Recognition>;

	partial class RecognizeBlocks
	{
		private static Recognition _ExtractStatementAndErrors(_RecognizeBlocks_Item content)
		{
			var rawContent = content.AsString();
			var coreContent = rawContent.TrimEnd();
			var extraAtEnd = rawContent.Substring(coreContent.Length);

			var errors = new List<ParseError>();
			var comments = new List<int>();
			var statement = _ExtractStatementAndComments(errors, coreContent, comments);
			_CheckForWhitespaceErrors(errors, statement, extraAtEnd);
			return Recognition.With(new UnknownStatement(statement, comments, errors.ToArray()));
		}

		private static Recognition _ExtractBlockAndErrors(_RecognizeBlocks_Item prelude, _RecognizeBlocks_Item body, _RecognizeBlocks_Item trailingWhitespace)
		{
			var coreContent = prelude.AsString();
			var extraAtEnd = trailingWhitespace.AsString();
			var preludeErrors = new List<ParseError>();
			_CheckForWhitespaceErrors(preludeErrors, coreContent, extraAtEnd);
			var blockErrors = Recognition.NoErrors;
			return new UnknownBlock(new UnknownPrelude(coreContent, preludeErrors), body.Results.SelectMany(r => r.Items), blockErrors).AsRecognition();
		}

		[NotNull]
		private static string _ExtractStatementAndComments([NotNull] List<ParseError> errors, [NotNull] string content, [NotNull] List<int> comments)
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
