using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	internal static class ParsingExtensions
	{
		[NotNull]
		public static string ToSetDisplayString([NotNull] this IEnumerable<string> values)
		{
			return "{" + string.Join(", ", values) + "}";
		}
	}
}
