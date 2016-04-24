using System;
using Gibberish.AST._1_Bare.Builders;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public static class BasicAst
	{
		[NotNull]
		public static BlankLineBuilder BlankLine(int indentationDepth)
		{
			return new BlankLineBuilder(PossiblySpecified<int>.WithValue(indentationDepth));
		}

		[NotNull]
		public static StatementBuilder Statement([NotNull] string content)
		{
			return new StatementBuilder(content, PossiblySpecified<int>.WithValue(0));
		}

		[NotNull]
		public static RawBlockBuilder RawBlock([NotNull] string prelude)
		{
			return RawBlock(prelude, x => { });
		}

		[NotNull]
		public static RawBlockBuilder RawBlock([NotNull] string prelude, [NotNull] Action<RawBlockBuilder.PreludeBuilder> preludeOptions)
		{
			return new RawBlockBuilder(prelude, preludeOptions, 0);
		}

		[NotNull]
		public static BlockBuilder Block([NotNull] string prelude)
		{
			return Block(prelude, x => { });
		}

		[NotNull]
		public static BlockBuilder Block([NotNull] string prelude, [NotNull] Action<BlockBuilder.PreludeBuilder> preludeOptions)
		{
			return new BlockBuilder(prelude, preludeOptions);
		}

		public static CommentBuilder CommentDefinition(int commentId, string content)
		{
			return new CommentBuilder(commentId, content);
		}
	}
}
