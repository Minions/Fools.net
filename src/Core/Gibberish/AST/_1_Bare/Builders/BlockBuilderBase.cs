using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public abstract class BlockBuilderBase<TBlockBuilder, TPreludeBuilder, TBodyBuilder> : AstBuilderSupportingErrors<LanguageConstruct>
		where TBlockBuilder : BlockBuilderBase<TBlockBuilder, TPreludeBuilder, TBodyBuilder>
		where TPreludeBuilder : BlockBuilderBase<TBlockBuilder, TPreludeBuilder, TBodyBuilder>.PreludeBuilderBase
		where TBodyBuilder : BlockBuilderBase<TBlockBuilder, TPreludeBuilder, TBodyBuilder>.BodyBuilderBase
	{
		protected BlockBuilderBase([NotNull] Action<TPreludeBuilder> preludeOptions, TPreludeBuilder preludeBuilder)
		{
			Prelude = preludeBuilder;
			preludeOptions(Prelude);
		}

		[NotNull]
		public TPreludeBuilder Prelude { get; }
		[NotNull]
		public List<AstBuilderSupportingErrors<LanguageConstruct>> Body { get; } = new List<AstBuilderSupportingErrors<LanguageConstruct>>();

		[NotNull]
		public TBlockBuilder WithBody([NotNull] Action<TBodyBuilder> bodyOptions)
		{
			bodyOptions(CreateBodyBuilder());
			return (TBlockBuilder) this;
		}

		public abstract class PreludeBuilderBase : AstBuilderSupportingErrors<LanguageConstruct>
		{
			protected PreludeBuilderBase(string content)
			{
				Content = content;
			}

			[NotNull]
			public string Content { get; }
			[NotNull]
			public List<int> Comments { get; } = new List<int>();

			[NotNull]
			public TPreludeBuilder WithCommentRefs([NotNull] params int[] indices)
			{
				Comments.AddRange(indices);
				return (TPreludeBuilder) this;
			}
		}

		public abstract class BodyBuilderBase
		{
			protected BodyBuilderBase([NotNull] TBlockBuilder self)
			{
				_self = self;
			}

			[NotNull]
			public TBlockBuilder AddBlock([NotNull] string prelude)
			{
				return AddBlock(prelude, _ => { });
			}

			[NotNull]
			public TBlockBuilder AddBlock([NotNull] string prelude, [NotNull] Action<TPreludeBuilder> preludeOptions)
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

			protected abstract TBlockBuilder CreateBlockBuilder(string prelude, Action<TPreludeBuilder> preludeOptions);

			[NotNull]
			private TBuilder _AddToBody<TBuilder>([NotNull] TBuilder result) where TBuilder : AstBuilderSupportingErrors<LanguageConstruct>
			{
				_self.Body.Add(result);
				return result;
			}

			[NotNull] protected readonly TBlockBuilder _self;
		}

		protected abstract TBodyBuilder CreateBodyBuilder();

		protected void _BuildBodyInto(List<LanguageConstruct> destination)
		{
			foreach (var builder in Body) { builder.BuildInto(destination); }
		}
	}
}
