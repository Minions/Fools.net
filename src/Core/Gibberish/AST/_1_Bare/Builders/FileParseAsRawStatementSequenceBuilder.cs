using System;

namespace Gibberish.AST._1_Bare.Builders
{
	public class FileParseAsRawStatementSequenceBuilder : FileParseBuilderBase<RawBlockBuilder, RawBlockBuilder.PreludeBuilder>
	{
		public FileParseAsRawStatementSequenceBuilder(Action<FileParseAsRawStatementSequenceBuilder> content)
		{
			content(this);
		}

		public override RawBlockBuilder Block(string prelude, Action<BlockBuilderBase.PreludeBuilderBase> preludeOptions)
		{
			return _Remember(new RawBlockBuilder(prelude, preludeOptions, 0));
		}

		public CommentDefinitionBlockPreludeBuilder CommentDefinitionBlockPrelude(int commentId)
		{
			return _Remember(new CommentDefinitionBlockPreludeBuilder(commentId));
		}

		public CommentDefinitionBlockStatementBuilder CommentDefinitionBlockStatement(string content)
		{
			return _Remember(new CommentDefinitionBlockStatementBuilder(content));
		}
	}
}
