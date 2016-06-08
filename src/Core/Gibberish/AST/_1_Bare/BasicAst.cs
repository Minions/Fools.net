using System;
using Gibberish.AST._1_Bare.Builders;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public static class BasicAst
	{
		[NotNull]
		public static FileParseBuilder SequenceOfRawLines(Action<FileParseBuilder> contents)
		{
			return new FileParseBuilderRaw(contents);
		}

		[NotNull]
		public static FileParseBuilder BlockTree(Action<FileParseBuilder> contents)
		{
			return new FileParseBuilderHierarchical(contents);
		}
	}
}
