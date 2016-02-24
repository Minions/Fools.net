using System.IO;
using ApprovalTests;
using MatchResultExtensions;
using NUnit.Framework;

namespace Gibberish.Tests.BeAbleToDefineAThunk
{
	[TestFixture]
	public class ParseThunkDefinition
	{
		[Test]
		public void definethunk_statement_should_match_whole_block()
		{
			var input = @"define.named.thunk some.name:
	pass
";
			var subject = new ParseFasm();
			var result = subject.GetMatch(input, subject.TopLevelStatement);
			Approvals.VerifyJson(result.PrettyPrint());
		}

		[Test]
		public void just_a_uselanguage_statement_should_be_empty_parse()
		{
			var input = "use language fasm\r\n";
			var subject = new ParseFasm();
			var result = subject.GetMatch(input, subject.FasmFile);
			Approvals.VerifyJson(result.PrettyPrint());
		}

		[Ignore("It will be a while.")]
		[Test]
		public void FullAcceptanceTest()
		{
			var contents = File.ReadAllText("...");

			// work hard to parse & stuff
			// emit something
			// assert that what was emitted is what we expected
		}
	}
}
