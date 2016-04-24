using System;
using Gibberish.AST._1_Bare.Builders;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public static class BasicAst
	{
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

		[NotNull]
		public static FileParseAsRawStatementSequenceBuilder SequenceOfRawLines(Action<FileParseAsRawStatementSequenceBuilder> contents)
		{
			return new FileParseAsRawStatementSequenceBuilder(contents);
		}
	}
}
