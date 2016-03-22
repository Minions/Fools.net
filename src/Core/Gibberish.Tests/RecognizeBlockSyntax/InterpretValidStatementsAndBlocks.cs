using Gibberish.AST;
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
			var expected = BasicAst.Statement("arbitrary statement");
			var subject = new RecognizeBlocks();
			var result = subject.GetMatch(input, subject.Statement);
			result.Should()
				.BeRecognizedAs(expected);
		}

		[Test]
		public void should_complain_about_invalid_whitespace_at_end_of_line()
		{
			var input = "arbitrary statement\t\r\n";
			var expected = BasicAst.Statement("arbitrary statement")
				.WithError(ParseError.IllegalWhitespaceAtEnd("\t"));
			var subject = new RecognizeBlocks();
			var result = subject.GetMatch(input, subject.Statement);
			result.Should()
				.BeRecognizedAs(expected);
		}
	}
}
