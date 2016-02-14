namespace Gibberish.AST
{
	public class FasmAst
	{
		public static ParseTree Thunk(string name, params Statement[] body) { return new DefineThunkNode(name, body); }

		public static Statement Pass { get; private set; } = new PassStatement();
	}
}
