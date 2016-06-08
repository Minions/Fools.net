using System;

namespace Gibberish.AST._1_Bare.Builders
{
	public class FileParseAsRawStatementSequenceBuilder : FileParseBuilderBase<BlockBuilderRaw>
	{
		public FileParseAsRawStatementSequenceBuilder(Action<FileParseAsRawStatementSequenceBuilder> content)
		{
			content(this);
		}

		public override BlockBuilderRaw Block(string prelude, Action<BlockBuilder.PreludeBuilder> preludeOptions)
		{
			return _Remember(new BlockBuilderRaw(prelude, preludeOptions, 0));
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
