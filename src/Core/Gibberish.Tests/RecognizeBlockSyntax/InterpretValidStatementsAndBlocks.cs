using Gibberish.AST._1_Bare;
using Gibberish.Parsing;
using Gibberish.Tests.ZzTestHelpers;
using NUnit.Framework;

namespace Gibberish.Tests.RecognizeBlockSyntax
{
	[TestFixture]
	public class InterpretValidStatementsAndBlocks
	{
		[Test]
		public void should_recognize_a_non_indented_statement()
		{
			var input = "arbitrary statement\r\n";
			var subject = new RecognizeBlocks();
			var result = subject.GetMatch(input, subject.Statement);
			result.Should()
				.BeRecognizedAs(BasicAst.Statement("arbitrary statement"));
		}
	}
}
