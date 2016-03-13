using FluentAssertions.Primitives;
using Gibberish.AST;
using IronMeta.Matcher;

namespace Gibberish.Tests.ZzTestHelpers
{
	public class ParseResultAssertions : ObjectAssertions
	{
		public ParseResultAssertions(MatchResult<char, Parse> value) : base(value)
		{
			Parse = value.Result;
		}

		public Parse Parse { get; }
	}
}
