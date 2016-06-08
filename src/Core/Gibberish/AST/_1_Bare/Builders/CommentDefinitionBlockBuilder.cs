using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class CommentDefinitionBlockBuilder : AstBuilderSupportingErrors<LanguageConstruct>
	{
		public CommentDefinitionBlockBuilder(int commentId, [NotNull] Action<CommentDefinitionBlockPreludeBuilder> preludeOptions)
		{
			Prelude = new CommentDefinitionBlockPreludeBuilder(commentId);
			preludeOptions(Prelude);
		}

		[NotNull]
		public CommentDefinitionBlockPreludeBuilder Prelude { get; }

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			var body = new List<LanguageConstruct>();
			Body.ForEach(b => b.BuildInto(body));
			destination.Add(
				new CommentDefinitionBlock(
					PossiblySpecified<bool>.WithValue(StartsParagraph),
					(CommentDefinitionBlockPrelude) Prelude.Build()
						.Single(),
					body.Cast<CommentDefinitionBlockStatement>(),
					Errors));
		}

		public class BodyBuilder
		{
			public BodyBuilder([NotNull] CommentDefinitionBlockBuilder self)
			{
				_self = self;
			}

			public CommentDefinitionBlockStatementBuilder AddStatement(string content)
			{
				var builder = new CommentDefinitionBlockStatementBuilder(content);
				_self.Body.Add(builder);
				return builder;
			}

			[NotNull] private readonly CommentDefinitionBlockBuilder _self;
		}

		[NotNull]
		public List<CommentDefinitionBlockStatementBuilder> Body { get; } = new List<CommentDefinitionBlockStatementBuilder>();

		[NotNull]
		public CommentDefinitionBlockBuilder WithBody([NotNull] Action<BodyBuilder> bodyOptions)
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
