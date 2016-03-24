using Gibberish.AST;
using Gibberish.Parsing;
using Gibberish.Tests.ZzTestHelpers;
using NUnit.Framework;

namespace Gibberish.Tests.CompileFools
{
	[TestFixture]
	public class CanGiveDecentSyntaxErrors
	{
		[Test]
		public void should_complain_about_extra_whitespace_at_end_of_line()
		{
			var input = "specify whatever:\t\r\n\tpass\r\n";
			var subject = new ParseFools();
			var result = subject.GetMatch(input, subject.Declarations);
			result.Should()
				.ParseWithErrors(ParseError.IllegalWhitespaceAtEnd("\t"));
		}
	}
}
