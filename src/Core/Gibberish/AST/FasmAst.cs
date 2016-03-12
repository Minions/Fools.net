using System.Collections.Generic;
using System.Linq;

namespace Gibberish.AST
{
	public class FasmAst
	{
		public static Parse Thunk(string name, params Statement[] body)
		{
			return Parse.Valid(new DefineThunkNode(name, body), Parse.NoErrors);
		}

		public static Statement Pass { get; private set; } = new PassStatement();
	}
}
