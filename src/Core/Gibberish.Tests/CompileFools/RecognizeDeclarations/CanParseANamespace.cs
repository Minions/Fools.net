using Gibberish.AST;
using Gibberish.Parsing;
using Gibberish.Tests.ZzTestHelpers;
using NUnit.Framework;

namespace Gibberish.Tests.CompileFools.RecognizeDeclarations
{
	[TestFixture]
	public class CanParseANamespace
	{
		[Test]
		public void should_recognize_an_empty_namespace()
		{
			var input = @"
in.namespace some.name:
	pass
";
			var subject = new ParseFools();
			var result = subject.GetMatch(input, subject.Declarations);
			result.Should()
				.ParseAs(FoolsAst.Namespace("some.name"));
		}
	}
}
