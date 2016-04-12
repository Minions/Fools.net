using System.Diagnostics.CodeAnalysis;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using Gibberish.AST._2_Fasm;
using Gibberish.Execution;
using Gibberish.Parsing;
using Gibberish.Tests.ZzTestHelpers;
using JetBrains.Annotations;
using NUnit.Framework;

namespace Gibberish.Tests.CompileFasm.BeAbleToDefineAThunk
{
	[TestFixture]
	public class CompileThunks
	{
		[Test]
		public void ShouldRecognizeADefineThunkBlock()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				BasicAst.RawBlock("define.named.thunk some.name")
					.WithBody(b => b.AddStatement("pass"))
					.Build());
			result.Should()
				.BeRecognizedAs(BasicAst.RawBlock("define.named.thunk some.name").WithBody(b => b.AddStatement("pass")));
		}

		[Test]
		public void CompilingATrivialDefineThunkNodeShouldCreateThunkInNameTable()
		{
			var testSubject = Tools.Fasm_Old.Compiler_OldApi;
			var parse = FasmAstOld.Thunk(ArbitraryName, FasmAstOld.PassRaw);
			testSubject.CompileFragment(parse, _arbitraryDistrict);
			_arbitraryDistrict.Name(ArbitraryName)
				.ShouldHave(
					new
					{
						name = ArbitraryName
					});
		}

		private const string ArbitraryName = "the.name";

		[NotNull, SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")] private City _city;
		[NotNull, SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")] private District _arbitraryDistrict;

		[SetUp]
		public void Setup()
		{
			_city = new City();
			_arbitraryDistrict = _city.District("CompileTest");
		}
	}
}
