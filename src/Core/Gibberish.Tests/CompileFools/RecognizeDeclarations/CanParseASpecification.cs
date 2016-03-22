using Gibberish.AST;
using Gibberish.Parsing;
using Gibberish.Tests.ZzTestHelpers;
using NUnit.Framework;

namespace Gibberish.Tests.CompileFools.RecognizeDeclarations
{
	[TestFixture]
	public class CanParseASpecification
	{
		[Test]
		public void should_recognize_an_empty_spec_section()
		{
			var input = @"
specify fasm compilation / thunks and messages:
	pass
";
			var subject = new ParseFools();
			var result = subject.GetMatch(input, subject.Declarations);
			result.Should()
				.ParseAs(FoolsAst.SpecificationSection("fasm compilation", "thunks and messages"));
		}
	}
}
