using System;

namespace Gibberish.AST._1_Bare.Builders
{
	public class FileParseAsForestOfBlocksBuilder : FileParseBuilderBase<BlockBuilder, BlockBuilder.PreludeBuilder>
	{
		public FileParseAsForestOfBlocksBuilder(Action<FileParseAsForestOfBlocksBuilder> content)
		{
			content(this);
		}

		public override BlankLineBuilder BlankLine()
		{
			return _Remember(new BlankLineBuilder(PossiblySpecified<int>.Unspecifed));
		}

		public override StatementBuilder Statement(string content)
		{
			return _Remember(new StatementBuilder(content, PossiblySpecified<int>.Unspecifed));
		}

		public override BlockBuilder Block(string prelude, Action<BlockBuilder.PreludeBuilder> preludeOptions)
		{
			return _Remember(new BlockBuilder(prelude, preludeOptions));
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
