using System.Collections.Generic;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Gibberish.Parsing.LineParsers
{
	internal static class ParseStatements
	{
		[NotNull]
		public static UnknownStatement Interpret(int indentationDepth, string content)
		{
			var coreContent = content.TrimEnd();
			var extraAtEnd = content.Substring(coreContent.Length);

			var errors = new List<ParseError>();
			var comments = new List<int>();
			var statement = ParseCommentReferences.ExtractCommentsAndReturnEverythingBeforeThem(errors, coreContent, comments);
			ParseCommentReferences.CheckForWhitespaceErrors(errors, statement, extraAtEnd);
			return new UnknownStatement(PossiblySpecified<bool>.Unspecifed, PossiblySpecified<int>.WithValue(indentationDepth), statement, comments, errors);
		}
	}
}
