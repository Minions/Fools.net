using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	public static class FasmAst
	{
		[NotNull]
		public static Parse Thunk([NotNull] string name, [NotNull] params Statement[] body)
		{
			return DefineThunkNode.From(name, body);
		}

		public static Statement PassRaw { get; } = new PassStatement();
		public static Parse Pass { get; } = Parse.Valid(PassRaw, Parse.NoErrors);
	}
}
