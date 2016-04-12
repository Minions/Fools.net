using System;
using Gibberish.AST._2_Fasm.Builders;

namespace Gibberish.AST._2_Fasm
{
	public static class FasmAst
	{
		public static NamedThunkBuilder NamedThunk(string name)
		{
return new NamedThunkBuilder(name);
		}
	}
}
