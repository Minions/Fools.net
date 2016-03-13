using ApprovalTests;
using Gibberish.Parsing;
using Gibberish.Tests.ZzTestHelpers;
using NUnit.Framework;

namespace Gibberish.Tests.BeAbleToDefineAThunk
{
	[TestFixture]
	public class ParseThunkDefinition
	{
		[Test, Timeout(1500)]
		public void definethunk_statement_should_match_whole_block()
		{
			var input = @"define.named.thunk some.name:
	pass
";
			var subject = new ParseFasm();
			var result = subject.GetMatch(input, subject.OneDeclaration);
			Approvals.VerifyJson(result.PrettyPrint());
		}
	}
}
