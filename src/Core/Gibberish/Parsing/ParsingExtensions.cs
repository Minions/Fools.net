using System.Collections.Generic;
using System.Linq;
using Gibberish.AST;
using IronMeta.Matcher;
using JetBrains.Annotations;

namespace Gibberish
{
	internal static class ParsingExtensions
	{
		[NotNull]
		public static Parse AsParse([NotNull] this ParseError parseErrors)
		{
			return Parse.WithErrors(parseErrors);
		}

		[NotNull]
		public static string ToSetDisplayString([NotNull] this IEnumerable<string> values)
		{
			return "{" + string.Join(", ", values) + "}";
		}

		[NotNull]
		public static Block ExtractBlock([NotNull] this MatchItem<char, Parse> matchItem)
		{
			return (Block) matchItem.Results.Single()
				.Statements.Single();
		}
	}
}
