using System;

namespace Gibberish.AST._1_Bare.Builders
{
	public class FileParseAsForestOfBlocksBuilder : FileParseBuilderBase
	{
		public FileParseAsForestOfBlocksBuilder(Action<FileParseAsForestOfBlocksBuilder> content)
		{
			content(this);
		}

		public override BlockBuilder Block(string prelude, Action<BlockBuilder.PreludeBuilder> preludeOptions)
		{
			return _Remember(new BlockBuilderHierarchical(prelude, preludeOptions));
		}

		public override CommentDefinitionBuilder CommentDefinition(int commentId, string content)
		{
			return _Remember(new CommentDefinitionBuilder(commentId, content));
		}

		public CommentDefinitionBlockBuilder CommentDefinitionBlock(int commentId)
		{
			return _Remember(new CommentDefinitionBlockBuilder(commentId, delegate { }));
		}
	}
}
