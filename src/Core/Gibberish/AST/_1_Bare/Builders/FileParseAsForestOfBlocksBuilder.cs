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
			return _Remember(new StatementBuilder(content, PossiblySpecified<int>.WithValue(0)));
		}

		public override BlockBuilder Block(string prelude, Action<BlockBuilder.PreludeBuilder> preludeOptions)
		{
			return _Remember(new BlockBuilder(prelude, preludeOptions));
		}

		public override BlockBuilder Block(string prelude)
		{
			return Block(prelude, x => { });
		}

		public override CommentBuilder CommentDefinition(int commentId, string content)
		{
			return _Remember(new CommentBuilder(commentId, content));
		}
	}
}