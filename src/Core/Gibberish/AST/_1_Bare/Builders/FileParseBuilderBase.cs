using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public abstract class FileParseBuilderBase<TBlockBuilder, TPreludeBuilder> : AstBuilder<LanguageConstruct>
	{
		[NotNull]
		public abstract BlankLineBuilder BlankLine();

		[NotNull]
		public abstract StatementBuilder Statement(string content);

		[NotNull]
		public abstract TBlockBuilder Block(string prelude, Action<TPreludeBuilder> preludeOptions);

		[NotNull]
		public abstract TBlockBuilder Block(string prelude);

		[NotNull]
		public abstract CommentDefinitionBuilder CommentDefinition(int commentId, string content);

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			foreach (var builder in _contents) { builder.BuildInto(destination); }
		}

		[NotNull]
		protected TBuilder _Remember<TBuilder>([NotNull] TBuilder line) where TBuilder : AstBuilderSupportingErrors<LanguageConstruct>
		{
			_contents.Add(line);
			return line;
		}

		[NotNull] private readonly List<AstBuilderSupportingErrors<LanguageConstruct>> _contents = new List<AstBuilderSupportingErrors<LanguageConstruct>>();
	}
}
