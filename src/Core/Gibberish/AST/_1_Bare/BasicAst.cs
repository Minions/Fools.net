using System;
using Gibberish.AST._1_Bare.Builders;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public static class BasicAst
	{
		[NotNull]
		public static FileParseAsRawStatementSequenceBuilder SequenceOfRawLines(Action<FileParseAsRawStatementSequenceBuilder> contents)
		{
			return new FileParseAsRawStatementSequenceBuilder(contents);
		}

		[NotNull]
		public static FileParseAsForestOfBlocksBuilder BlockTree(Action<FileParseAsForestOfBlocksBuilder> contents)
		{
			return new FileParseAsForestOfBlocksBuilder(contents);
		}
	}
}
