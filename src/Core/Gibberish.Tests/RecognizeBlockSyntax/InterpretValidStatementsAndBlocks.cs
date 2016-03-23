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
		public void should_recognize_as(string input, BasicAst.Builder expected)
		{
			var subject = new RecognizeBlocks();
			var result = subject.GetMatch(input, subject.LanguageConstruct);
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
			},
			new object[]
			{
				"arbitrary\tstatement\r\n",
				BasicAst.Statement("arbitrary\tstatement")
					.WithError(ParseError.IllegalTabInLine())
			},
			new object[]
			{
				"arbitrary block:\r\n\tpass\r\n",
				BasicAst.Block("arbitrary block")
					.WithBody(b => b.AddStatement("pass"))
			},
			new object[]
			{
				"arbitrary block:\r\n\tpass \r\n",
				BasicAst.Block("arbitrary block")
					.WithBody(
						b => b.AddStatement("pass")
							.WithError(ParseError.IllegalWhitespaceAtEnd(" ")))
			},
			new object[]
			{
				"arbitrary block: \r\n\tpass\r\n",
				BasicAst.Block("arbitrary block", p => p.WithError(ParseError.IllegalWhitespaceAtEnd(" ")))
					.WithBody(b => b.AddStatement("pass"))
			},
			new object[]
			{
				"arbitrary block:\t\r\n\tpass\r\n",
				BasicAst.Block("arbitrary block", p => p.WithError(ParseError.IllegalWhitespaceAtEnd("\t")))
					.WithBody(b => b.AddStatement("pass"))
			},
			new object[]
			{
				"arbitrary block\t:\r\n\tpass\r\n",
				BasicAst.Block("arbitrary block\t", p => p.WithError(ParseError.IllegalTabInLine()))
					.WithBody(b => b.AddStatement("pass"))
			},
			new object[]
			{
				"arbitrary\tblock:\r\n\tpass\r\n",
				BasicAst.Block("arbitrary\tblock", p => p.WithError(ParseError.IllegalTabInLine()))
					.WithBody(b => b.AddStatement("pass"))
			}
		};
	}
}
