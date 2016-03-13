using System.Collections.Generic;
using Gibberish.AST;
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
	}
}
