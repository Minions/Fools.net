using Gibberish.AST;
using Gibberish.Execution;
using JetBrains.Annotations;

namespace Gibberish
{
	public abstract class Compiler_Old
	{
		public abstract void CompileFragment([NotNull] Parse parse, [NotNull] District @where);
	}
}
