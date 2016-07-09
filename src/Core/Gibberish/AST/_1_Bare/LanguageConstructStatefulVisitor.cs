namespace Gibberish.AST._1_Bare
{
	public interface LanguageConstructStatefulVisitor
	{
		void Visit(BlankLine line);

		void Visit(UnknownStatement statement);

		void Visit(CommentDefinition commentDefinition);

		void Visit(UnknownPrelude prelude);

		void Visit(UnknownBlock block);

		void Visit(CommentDefinitionBlockPrelude prelude);

		void Visit(CommentDefinitionBlockStatement statement);

		void Visit(CommentDefinitionBlock commentDefinition);
	}
}
