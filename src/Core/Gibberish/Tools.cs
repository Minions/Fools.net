using Gibberish.Execution;
using JetBrains.Annotations;

namespace Gibberish
{
	public static class Tools
	{
		[NotNull]
		public static LanguageTools Fasm_Old { get; private set; } = new LanguageTools(new FasmCompiler_Old());
	}
}
