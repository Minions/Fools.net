using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public abstract class FileParseBuilderBase<TBlockBuilder> : AstBuilder<LanguageConstruct>
	{
		[NotNull]
		public abstract TBlockBuilder Block(string prelude, Action<BlockBuilder.PreludeBuilder> preludeOptions);

		[NotNull]
		public TBlockBuilder Block(string prelude)
		{
			return Block(prelude, x => { });
		}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			foreach (var builder in _contents) { builder.BuildInto(destination); }
		}

		[NotNull]
		public virtual BlankLineBuilder BlankLine()
		{
			return _Remember(new BlankLineBuilder(PossiblySpecified<int>.Unspecifed));
		}

		[NotNull]
		public BlankLineBuilder BlankLine(int indentationDepth)
		{
			return _Remember(new BlankLineBuilder(PossiblySpecified<int>.WithValue(indentationDepth)));
		}

		[NotNull]
		public virtual StatementBuilder Statement([NotNull] string content)
		{
			return _Remember(new StatementBuilder(content, PossiblySpecified<int>.WithValue(0)));
		}

		[NotNull]
		public virtual CommentDefinitionBuilder CommentDefinition(int commentId, [NotNull] string content)
		{
			return _Remember(new CommentDefinitionBuilder(commentId, content));
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
