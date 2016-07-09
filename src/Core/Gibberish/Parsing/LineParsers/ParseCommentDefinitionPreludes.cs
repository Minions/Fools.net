using System.Collections.Generic;
using System.Text.RegularExpressions;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using Gibberish.Parsing.Passes;
using JetBrains.Annotations;

namespace Gibberish.Parsing.LineParsers
{
	internal static class ParseCommentDefinitionPreludes
	{
		public static LanguageConstruct Interpret(int indentationDepth, string content, RecognizeLines recognizeLines)
		{
			recognizeLines.RememberThatHaveFoundCommentSection();
			var match = CommentDefinitionBlockPreludePattern.Match(content);
			if (!match.Success) { return _ExtractCommentDefinitionBlockPrelude(indentationDepth, "", ""); }
			var commentId = match.Groups["commentId"].Value;
			var extra = match.Groups["extra"].Value;
			return _ExtractCommentDefinitionBlockPrelude(indentationDepth, commentId, extra);
		}

		public static bool Matches(string content)
		{
			return content.StartsWith("##");
		}

		private static LanguageConstruct _ExtractCommentDefinitionBlockPrelude(int indentationDepth, string commentId, string extra)
		{
			var errors = new List<ParseError>();

			if (!string.IsNullOrEmpty(extra)) { errors.Add(ParseError.IllegalContentAfterColonInPrelude(extra)); }

			int commentNumber;
			if (!int.TryParse(commentId, out commentNumber)) { errors.Add(ParseError.MissingIdInCommentDefinition(commentId.Substring(0, 8))); }

			return new CommentDefinitionBlockPrelude(commentNumber, errors);
		}

		[NotNull] private static readonly Regex CommentDefinitionBlockPreludePattern = new Regex(@"(?x)
				^\#\#
					\[(?<commentId>[0-9]+)\]\:(?<extra>.*)
", RegexOptions.Compiled);
	}
}
