using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class CommentDefinitionBlockBodyBuilder
	{
		public CommentDefinitionBlockBodyBuilder([NotNull] CommentDefinitionBlockBuilder self)
		{
			_self = self;
		}

		[NotNull]
		public CommentDefinitionBlockStatementBuilder AddStatement([NotNull] string content)
		{
			var builder = new CommentDefinitionBlockStatementBuilder(content);
			_self.Body.Add(builder);
			return builder;
		}

		[NotNull] private readonly CommentDefinitionBlockBuilder _self;
	}
}
