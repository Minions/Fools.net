using System.Collections.Generic;

namespace Lair.StringExtensions
{
	internal static class _
	{
		public static string JoinString<T>(this IEnumerable<T> @this, string separator)
		{
			return string.Join(separator, @this);
		}
	}
}
