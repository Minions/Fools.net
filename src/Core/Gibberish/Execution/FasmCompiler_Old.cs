using System.Linq;
using Gibberish.AST;

namespace Gibberish.Execution
{
	internal class FasmCompiler_Old : Compiler_Old
	{
		public override void CompileFragment(Parse parse, District @where)
		{
			var defineThunkNode = parse.Declarations.Cast<DefineThunkNode>()
				.Single();
			where.define_name(new ThunkDescriptor(defineThunkNode.name));
		}
	}
}
