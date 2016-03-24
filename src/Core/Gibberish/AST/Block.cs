using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	internal class Block : Statement
	{
		public Block([NotNull] Parse prelude, [NotNull] IEnumerable<Parse> body)
		{
			Prelude = prelude;
			Body = body;
		}

		[NotNull] public readonly Parse Prelude;
		[NotNull] public readonly IEnumerable<Parse> Body;
	}
}
