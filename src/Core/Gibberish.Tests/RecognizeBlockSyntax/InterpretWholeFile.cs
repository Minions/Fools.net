using ApprovalTests.Reporters;
using Gibberish.AST._1_Bare;
using Gibberish.Parsing;
using Gibberish.Tests.ZzTestHelpers;
using NUnit.Framework;

namespace Gibberish.Tests.RecognizeBlockSyntax
{
	[TestFixture]
	public class InterpretWholeFile
	{
		[Test, UseReporter(typeof(QuietReporter))]
		public void should_accept_multiple_language_constructs()
		{
			var subject = new RecognizeBlocks();
			var input = @"
using language fasm

define.thunk some.name:
	pass

define.thunk other.name:
	pass
";
			var result = subject.GetMatch(input, subject.WholeFile);
			//ApprovalTests.Approvals.VerifyJson(result.PrettyPrint());
			result.Should()
				.BeRecognizedAs(
					BasicAst.Statement("using language fasm"),
					BasicAst.Block("define.thunk some.name")
						.WithBody(b => b.AddStatement("pass")),
					BasicAst.Block("define.thunk other.name")
						.WithBody(b => b.AddStatement("pass")));
		}
	}
}
