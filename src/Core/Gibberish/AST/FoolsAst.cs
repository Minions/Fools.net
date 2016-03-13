using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	public static class FoolsAst
	{
		[NotNull]
		public static Declaration Namespace([NotNull] string name)
		{
			return new Namespace(name, Enumerable.Empty<Declaration>());
		}
	}
}
