using System.Collections.Generic;

namespace Gibberish.AST
{
	public class DeclarationSet : ParseTree
	{
		public readonly string type = "declarations";
		public List<Statement> declarations = new List<Statement>();
	}
}