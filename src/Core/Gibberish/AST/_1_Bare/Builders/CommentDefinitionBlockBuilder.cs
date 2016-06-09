using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public abstract class CommentDefinitionBlockBuilder : AstBuilderSupportingErrors<LanguageConstruct>
	{
		protected CommentDefinitionBlockBuilder(int commentId, [NotNull] Action<CommentDefinitionBlockPreludeBuilder> preludeOptions)
		{
			Prelude = new CommentDefinitionBlockPreludeBuilder(commentId);
			preludeOptions(Prelude);
		}

		[NotNull]
		public CommentDefinitionBlockPreludeBuilder Prelude { get; }
		[NotNull]
		public List<CommentDefinitionBlockStatementBuilder> Body { get; } = new List<CommentDefinitionBlockStatementBuilder>();
		public bool StartsParagraph { get; private set; }

		[NotNull]
		public CommentDefinitionBlockBuilder WithBody([NotNull] Action<CommentDefinitionBlockBodyBuilder> bodyOptions)
		{
			bodyOptions(new CommentDefinitionBlockBodyBuilder(this));
			return this;
		}

		public CommentDefinitionBlockBuilder ThatStartsParagraph()
		{
			StartsParagraph = true;
			return this;
		}
	}
}
