using System.Collections.Generic;

namespace Gibberish.AST
{
	internal class Block : ParseTree
	{
		public Block(ParseTree prelude, IEnumerable<Statement> statements)
		{
			Prelude = prelude;
			Statements = statements;
		}

		public ParseTree Prelude;
		public IEnumerable<Statement> Statements;
	}
}
