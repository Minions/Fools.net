using System;

namespace Gibberish.AST._1_Bare.Builders
{
	public class FileParseBuilderRaw : FileParseBuilder
	{
		public FileParseBuilderRaw(Action<FileParseBuilder> content)
		{
			content(this);
		}

		public override BlockBuilder Block(string prelude, Action<BlockBuilder.PreludeBuilder> preludeOptions)
		{
			return _Remember(new BlockBuilderRaw(prelude, preludeOptions, 0));
		}

		public override CommentDefinitionBlockBuilder CommentDefinitionBlock(int commentId)
		{
			return _Remember(new CommentDefinitionBlockBuilderRaw(commentId, delegate { }));
		}
	}
}
