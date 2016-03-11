using ApprovalTests;
using Gibberish.Tests.ZzTestHelpers;
using NUnit.Framework;

namespace Gibberish.Tests.PickTheRightLanguageForFile
{
	[TestFixture]
	public class ParserChooseLanguageBasedOnUseLanguageStatement
	{
		[Test]
		public void just_a_uselanguage_statement_should_be_empty_parse()
		{
			var input = "use language fasm\r\n";
			var subject = new ParseFasm();
			var result = subject.GetMatch(input, subject.FasmFile);
			Approvals.VerifyJson(result.PrettyPrint());
		}
	}
}
