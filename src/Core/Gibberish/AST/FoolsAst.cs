using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	public static class FoolsAst
	{
		[NotNull]
		public static Parse Namespace([NotNull] string name)
		{
			return Parse.Valid(new Namespace(name, Enumerable.Empty<Declaration>()), Parse.NoErrors);
		}
	}
}
