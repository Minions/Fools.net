using System;

namespace Gibberish.AST._1_Bare.Builders
{
	public class FileParseBuilderHierarchical : FileParseBuilder
	{
		public FileParseBuilderHierarchical(Action<FileParseBuilder> content)
		{
			content(this);
		}

		public override BlockBuilder Block(string prelude, Action<BlockBuilder.PreludeBuilder> preludeOptions)
		{
			return _Remember(new BlockBuilderHierarchical(prelude, preludeOptions));
		}

		public override CommentDefinitionBlockBuilder CommentDefinitionBlock(int commentId)
		{
			return _Remember(new CommentDefinitionBlockBuilderHierarchical(commentId, delegate { }));
		}
	}
}
