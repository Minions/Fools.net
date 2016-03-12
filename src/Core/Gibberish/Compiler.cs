using Gibberish.AST;
using Gibberish.Execution;
using JetBrains.Annotations;

namespace Gibberish
{
	public abstract class Compiler
	{
		[NotNull]
		public abstract City CreateCity();

		public abstract void CompileFragment([NotNull] Parse parse, [NotNull] District @where);
	}
}
