using System.Collections.Generic;

namespace Gibberish.AST
{
	internal class Block : Statement
	{
		public Block(Parse prelude, IEnumerable<Parse> body)
		{
			Prelude = prelude;
			Body = body;
		}

		public readonly Parse Prelude;
		public readonly IEnumerable<Parse> Body;
	}
}
