using System.Collections.Generic;
using System.Text.RegularExpressions;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Gibberish.Parsing
{
	public static class ParseCommentDefinitions
	{
		public static bool IsCommentDefinition(string content)
		{
			return content.StartsWith("#");
		}

		public static LanguageConstruct InterpretCommentDefinition(RecognizeLines recognizeLines, string content)
		{
			recognizeLines.RememberThatHaveFoundCommentSection();
			var match = CommentDefinitionPattern.Match(content);
			if (!match.Success)
			{
				return _ExtractSingleLineCommentDefinition(
					"",
					content.Substring(1)
						.TrimStart(),
					"",
					RecognizeLines.CRLF);
			}
			var commentId = match.Groups["commentId"].Value;
			var commentSeparator = match.Groups["commentSeparator"].Value;
			var firstLineContent = match.Groups["firstLineContent"].Value;

			return _ExtractSingleLineCommentDefinition(commentId, firstLineContent, commentSeparator, RecognizeLines.CRLF);
		}

		[NotNull]
		private static LanguageConstruct _ExtractSingleLineCommentDefinition(string commentId, string content, string commentSeparator, string newline)
		{
			var errors = new List<ParseError>();
			RecognizeLines.RequireNewline(newline, errors);
			var commentNumber = _ExtractCommentNumber(commentId, content, commentSeparator, errors);
			return new CommentDefinition(PossiblySpecified<bool>.Unspecifed, commentNumber, content, errors);
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

		[NotNull] private static readonly Regex CommentDefinitionPattern = new Regex(@"(?x)^\#
				\[(?<commentId>[0-9]+)\]\:
				(?<commentSeparator>\s+)
				(?<firstLineContent>.*)
", RegexOptions.Compiled);
	}
}
