using JetBrains.Annotations;

namespace Gibberish
{
	public class LanguageTools
	{
		public LanguageTools([NotNull] Compiler_Old compilerOldApi) { Compiler_OldApi = compilerOldApi; }

		[NotNull]
		public Compiler_Old Compiler_OldApi { get; private set; }
	}
}