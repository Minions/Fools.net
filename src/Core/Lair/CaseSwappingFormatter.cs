using System.Linq;
using static System.Char;

namespace Lair
{
	public class CaseSwappingFormatter
	{
		public string Format(string value)
		{
			return new string(
				value.Select(SwapCase)
					.ToArray());
		}

		private static char SwapCase(char c)
		{
			if (c != ToUpper(c)) { return ToUpper(c); }
			if (c != ToLower(c)) { return ToLower(c); }
			return c;
		}
	}
}
