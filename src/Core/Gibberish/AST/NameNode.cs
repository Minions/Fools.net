namespace Gibberish.AST
{
	internal class NameNode : ParseTree
	{
		public NameNode(string value) { Name = value; }

		public string Name { get; }
	}
}
