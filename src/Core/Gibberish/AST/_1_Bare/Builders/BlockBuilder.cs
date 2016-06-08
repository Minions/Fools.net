using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public abstract class BlockBuilder : AstBuilderSupportingErrors<LanguageConstruct>
	{
		protected BlockBuilder([NotNull] Action<PreludeBuilder> preludeOptions, PreludeBuilder preludeBuilder)
		{
			Prelude = preludeBuilder;
			preludeOptions(Prelude);
		}

		[NotNull]
		public PreludeBuilder Prelude { get; }
		[NotNull]
		public List<AstBuilderSupportingErrors<LanguageConstruct>> Body { get; } = new List<AstBuilderSupportingErrors<LanguageConstruct>>();
		public bool StartsParagraph { get; private set; }

		[NotNull]
		public BlockBuilder WithBody([NotNull] Action<BodyBuilder> bodyOptions)
		{
			bodyOptions(_CreateBodyBuilder());
			return this;
		}

		public abstract class PreludeBuilder : AstBuilderSupportingErrors<LanguageConstruct>
		{
			protected PreludeBuilder(string content)
			{
				Content = content;
			}

			[NotNull]
			public string Content { get; }
			[NotNull]
			public List<int> Comments { get; } = new List<int>();
			public abstract PossiblySpecified<int> IndentationDepth { get; }

			[NotNull]
			public PreludeBuilder WithCommentRefs([NotNull] params int[] indices)
			{
				Comments.AddRange(indices);
				return this;
			}

			public override void BuildInto(List<LanguageConstruct> destination)
			{
				destination.Add(new UnknownPrelude(IndentationDepth, Content, Comments, Errors));
			}
		}

		public abstract class BodyBuilder
		{
			protected BodyBuilder([NotNull] BlockBuilder self)
			{
				_self = self;
			}

			[NotNull]
			public BlockBuilder AddBlock([NotNull] string prelude)
			{
				return AddBlock(prelude, _ => { });
			}

			[NotNull]
			public BlockBuilder AddBlock([NotNull] string prelude, [NotNull] Action<PreludeBuilder> preludeOptions)
			{
				return _AddToBody(CreateBlockBuilder(prelude, preludeOptions));
			}

			public abstract PossiblySpecified<int> IndentationDepth { get; }

			[NotNull]
			public StatementBuilder AddStatement([NotNull] string content)
			{
				return _AddToBody(new StatementBuilder(content, IndentationDepth));
			}

			[NotNull]
			public BlankLineBuilder AddBlankLine()
			{
				return _AddToBody(new BlankLineBuilder(IndentationDepth));
			}

			protected abstract BlockBuilder CreateBlockBuilder(string prelude, Action<PreludeBuilder> preludeOptions);

			[NotNull]
			private TBuilder _AddToBody<TBuilder>([NotNull] TBuilder result) where TBuilder : AstBuilderSupportingErrors<LanguageConstruct>
			{
				_self.Body.Add(result);
				return result;
			}

			[NotNull] protected readonly BlockBuilder _self;
		}

		public BlockBuilder ThatStartsNewParagraph()
		{
			StartsParagraph = true;
			return this;
		}

		protected abstract BodyBuilder _CreateBodyBuilder();

		protected void _BuildBodyInto(List<LanguageConstruct> destination)
		{
			foreach (var builder in Body) { builder.BuildInto(destination); }
		}
	}
}
