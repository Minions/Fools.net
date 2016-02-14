using JetBrains.Annotations;

namespace Gibberish
{
	public class LanguageTools
	{
		public LanguageTools([NotNull] Compiler compiler) { Compiler = compiler; }

		[NotNull]
		public Compiler Compiler { get; private set; }
	}
}