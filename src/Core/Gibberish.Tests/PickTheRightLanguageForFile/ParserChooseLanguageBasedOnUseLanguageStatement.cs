using Gibberish.AST;
using Gibberish.Parsing;
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
			var subject = new ParseLanguageFile();
			var result = subject.GetMatch(input, subject.File);
			result.Should()
				.ParseAs(Parse.Empty);
		}

		[Test]
		public void use_language_fasm_should_be_able_to_get_empty_thunk_def_in_that_language()
		{
			var input = @"use language fasm

define.named.thunk some.name:
	pass
";
			var subject = new ParseLanguageFile();
			var result = subject.GetMatch(input, subject.File);
			result.Should()
				.ParseAs(FasmAst.Thunk("some.name", FasmAst.PassRaw));
		}

		[Test]
		public void use_language_fools_should_be_able_to_get_empty_namespace_in_that_language()
		{
			var input = @"use language fools

in.namespace the.name:
	pass
";
			var subject = new ParseLanguageFile();
			var result = subject.GetMatch(input, subject.File);
			result.Should()
				.ParseAs(FoolsAst.Namespace("the.name"));
		}

		[Test]
		public void unknown_lang_should_give_nice_error()
		{
			var input = "use language unknown\r\n";
			var subject = new ParseLanguageFile();
			var result = subject.GetMatch(input, subject.File);
			result.Should()
				.ParseWithErrors(ParseError.UnknownLanguage("unknown"));
		}

		[Test, Ignore("Not implemented; should be in lang spec.")]
		public void missing_use_language_should_give_nice_error() {}
	}
}
