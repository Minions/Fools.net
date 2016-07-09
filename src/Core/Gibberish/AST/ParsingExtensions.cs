using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	internal static class ParsingExtensions
	{
		[NotNull]
		public static string ToSetDisplayString([NotNull] this IEnumerable<string> values)
		{
			return "{" + String.Join(", ", values) + "}";
		}

		public static string FirstUpToNChars(this string content, int numChars)
		{
			return content.Substring(0, Math.Min(numChars, content.Length));
		}
	}
}
