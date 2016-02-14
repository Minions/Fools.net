using FluentAssertions;
using Gibberish.AST;
using Gibberish.Execution;
using NUnit.Framework;

namespace Gibberish.Tests.BeAbleToDefineAThunk
{
	[TestFixture]
	public class CompileThunks
	{
		[Test]
		public void CompilingATrivialDefineThunkNodeShouldCreateThunkInNameTable()
		{
			var testSubject = Tools.Fasm.Compiler;
			var city = new City();
			var district = city.District("CompileTest");
			var parse = FasmAst.Thunk("the.name", FasmAst.Pass);
			testSubject.CompileFragment(parse, district);
			district.Name("the.name")
				.Should()
				.NotBeNull();
		}
	}
}
