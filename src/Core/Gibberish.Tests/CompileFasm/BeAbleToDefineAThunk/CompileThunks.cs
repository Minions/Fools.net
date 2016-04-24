using System.Diagnostics.CodeAnalysis;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
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
		public void ShouldAssembleBlocksfromValidNesting()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				BasicAst.RawBlock("outer block")
					.WithBody(
						b =>
						{
							b.AddStatement("outer 1");
							b.AddBlock("nested 1")
								.WithBody(inner => inner.AddStatement("nested 1.1"));
							b.AddStatement("outer 2");
							b.AddBlock("nested 2")
								.WithBody(
									inner =>
									{
										inner.AddStatement("nested 2.1");
										inner.AddStatement("nested 2.2");
									});
							b.AddBlock("nested 3")
								.WithBody(inner => inner.AddStatement("nested 3.1"));
						})
					.Build());
			result.Should()
				.BeRecognizedAs(
					BasicAst.Block("outer block")
						.WithBody(
							b =>
							{
								b.AddStatement("outer 1");
								b.AddBlock("nested 1")
									.WithBody(inner => inner.AddStatement("nested 1.1"));
								b.AddStatement("outer 2");
								b.AddBlock("nested 2")
									.WithBody(
										inner =>
										{
											inner.AddStatement("nested 2.1");
											inner.AddStatement("nested 2.2");
										});
								b.AddBlock("nested 3")
									.WithBody(inner => inner.AddStatement("nested 3.1"));
							}));
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
