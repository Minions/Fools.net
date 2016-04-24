using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public abstract class FileParseBuilderBase : AstBuilder<LanguageConstruct>
	{
		[NotNull]
		public abstract BlankLineBuilder BlankLine(int indentationDepth);

		[NotNull]
		public abstract StatementBuilder Statement(string content);

		[NotNull]
		public abstract RawBlockBuilder Block(string prelude, Action<RawBlockBuilder.PreludeBuilder> preludeOptions);

		[NotNull]
		public abstract RawBlockBuilder Block(string prelude);

		[NotNull]
		public abstract CommentBuilder CommentDefinition(int commentId, string content);

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			foreach (var builder in _contents) { builder.BuildInto(destination); }
		}

		[NotNull]
		protected TBuilder _Remember<TBuilder>([NotNull] TBuilder line) where TBuilder : AstBuilder<LanguageConstruct>
		{
			_contents.Add(line);
			return line;
		}

		[NotNull] private readonly List<AstBuilder<LanguageConstruct>> _contents = new List<AstBuilder<LanguageConstruct>>();
	}
}
