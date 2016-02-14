using System;
using Gibberish.AST;

namespace Gibberish.Execution
{
	internal class FasmCompiler : Compiler
	{
		public override City CreateCity() { return new City(); }

		public override void CompileFragment(ParseTree parse, District @where)
		{
			var defineThunkNode = parse as DefineThunkNode;
			where.define_name(new ThunkDescriptor(defineThunkNode.name));
		}
	}
}
