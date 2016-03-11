using System.Collections.Generic;
using System.Linq;
using IronMeta.Matcher;

namespace Gibberish.AST
{
	public class DeclarationSet : ParseTree
	{
		public readonly string type = "declarations";
		public List<Statement> declarations;

		public DeclarationSet(IEnumerable<ParseTree> declarations)
		{
			this.declarations = declarations.Cast<Statement>().ToList();
		}
	}
}