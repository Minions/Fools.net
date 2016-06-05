using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class CommentBlockBuilder : AstBuilderSupportingErrors<LanguageConstruct>
	{
		public CommentBlockBuilder(int commentId, [NotNull] Action<MultilineCommentPreludeBuilder> preludeOptions)
		{
			Prelude = new MultilineCommentPreludeBuilder(commentId);
			preludeOptions(Prelude);
		}

		[NotNull]
		public MultilineCommentPreludeBuilder Prelude { get; }

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			var body = new List<LanguageConstruct>();
			Body.ForEach(b => b.BuildInto(body));
			destination.Add(
				new CommentDefinitionBlock(
					PossiblySpecified<bool>.WithValue(StartsParagraph),
					(MultilineCommentDefinitionPrelude) Prelude.Build()
						.Single(),
					body.Cast<MultilineCommentDefinitionStatement>(),
					Errors));
		}

		public class BodyBuilder
		{
			public BodyBuilder([NotNull] CommentBlockBuilder self)
			{
				_self = self;
			}

			public MultilineCommentStatementBuilder AddStatement(string content)
			{
				var builder = new MultilineCommentStatementBuilder(content);
				_self.Body.Add(builder);
				return builder;
			}

			[NotNull] private readonly CommentBlockBuilder _self;
		}

		[NotNull]
		public List<MultilineCommentStatementBuilder> Body { get; } = new List<MultilineCommentStatementBuilder>();

		[NotNull]
		public CommentBlockBuilder WithBody([NotNull] Action<BodyBuilder> bodyOptions)
		{
			bodyOptions(CreateBodyBuilder());
			return this;
		}

		public bool StartsParagraph { get; private set; }

		public void ThatStartsParagraph()
		{
			StartsParagraph = true;
		}

		private BodyBuilder CreateBodyBuilder()
		{
			return new BodyBuilder(this);
		}
	}
}
