using System;
using System.Linq;
using Gibberish.AST;

namespace Gibberish.Execution
{
	internal class FasmCompiler : Compiler
	{
		public override City CreateCity() { return new City(); }

		public override void CompileFragment(Parse parse, District @where)
		{
			var defineThunkNode = parse.Declarations.Cast<DefineThunkNode>().Single();
			where.define_name(new ThunkDescriptor(defineThunkNode.name));
		}
	}
}
