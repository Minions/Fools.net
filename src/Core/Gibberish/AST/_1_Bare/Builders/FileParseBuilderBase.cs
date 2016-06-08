using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public abstract class FileParseBuilderBase : AstBuilder<LanguageConstruct>
	{
		[NotNull]
		public abstract BlockBuilder Block(string prelude, Action<BlockBuilder.PreludeBuilder> preludeOptions);

		[NotNull]
		public BlockBuilder Block(string prelude)
		{
			return Block(prelude, x => { });
		}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			foreach (var builder in _contents) { builder.BuildInto(destination); }
		}

		[NotNull]
		public BlankLineBuilder BlankLine(int indentationDepth = 0)
		{
			return _Remember(new BlankLineBuilder(PossiblySpecified<int>.WithValue(indentationDepth)));
		}

		[NotNull]
		public StatementBuilder Statement([NotNull] string content, int indentationDepth = 0)
		{
			return _Remember(new StatementBuilder(content, PossiblySpecified<int>.WithValue(indentationDepth)));
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
