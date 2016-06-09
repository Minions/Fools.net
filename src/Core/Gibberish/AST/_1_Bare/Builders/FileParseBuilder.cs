using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public abstract class FileParseBuilder : AstBuilder<LanguageConstruct>
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
		public CommentDefinitionBuilder CommentDefinition(int commentId, [NotNull] string content)
		{
			return _Remember(new CommentDefinitionBuilder(commentId, content));
		}

		[NotNull]
		public CommentDefinitionBlockStatementBuilder IllegalCommentBlockStatement(int indentationDepth, [NotNull] string content)
		{
			return _Remember(new CommentDefinitionBlockStatementBuilder(content).WithIndentationDepth(indentationDepth));
		}

		public CommentDefinitionBlockBuilder CommentDefinitionBlock(int commentId)
		{
			return CommentDefinitionBlock(commentId, delegate { });
		}

		public abstract CommentDefinitionBlockBuilder CommentDefinitionBlock(int commentId, Action<CommentDefinitionBlockPreludeBuilder> preludeOptions);

		[NotNull]
		protected TBuilder _Remember<TBuilder>([NotNull] TBuilder line) where TBuilder : AstBuilderSupportingErrors<LanguageConstruct>
		{
			_contents.Add(line);
			return line;
		}

		[NotNull] private readonly List<AstBuilderSupportingErrors<LanguageConstruct>> _contents = new List<AstBuilderSupportingErrors<LanguageConstruct>>();
	}
}
