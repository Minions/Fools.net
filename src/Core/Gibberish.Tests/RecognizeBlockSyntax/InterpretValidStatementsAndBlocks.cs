using System.Collections.Generic;
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
		[Test, TestCaseSource(nameof(valid_recognitions))]
		public void should_recognize_as(string input, BasicAst.StatementBuilder expected)
		{
			var subject = new RecognizeBlocks();
			var result = subject.GetMatch(input, subject.Statement);
			result.Should()
				.BeRecognizedAs(expected);
		}

		public static IEnumerable<IEnumerable<object>> valid_recognitions { get; } = new[]
		{
			new object[]
			{
				"arbitrary statement\r\n",
				BasicAst.Statement("arbitrary statement")
			},
			new object[]
			{
				"arbitrary statement \r\n",
				BasicAst.Statement("arbitrary statement")
					.WithError(ParseError.IllegalWhitespaceAtEnd(" "))
			},
			new object[]
			{
				"arbitrary statement\t\r\n",
				BasicAst.Statement("arbitrary statement")
					.WithError(ParseError.IllegalWhitespaceAtEnd("\t"))
			}
		};
	}
}
