using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public abstract class BlockBuilderBase<TBlockBuilder, TPreludeBuilder, TBodyBuilder, TLang> : AstBuilderSupportingErrors<TLang>
		where TBlockBuilder : BlockBuilderBase<TBlockBuilder, TPreludeBuilder, TBodyBuilder, TLang>
		where TPreludeBuilder : BlockBuilderBase<TBlockBuilder, TPreludeBuilder, TBodyBuilder, TLang>.PreludeBuilderBase
		where TBodyBuilder : BlockBuilderBase<TBlockBuilder, TPreludeBuilder, TBodyBuilder, TLang>.BodyBuilderBase
	{
		protected BlockBuilderBase([NotNull] Action<TPreludeBuilder> preludeOptions, TPreludeBuilder preludeBuilder)
		{
			Prelude = preludeBuilder;
			preludeOptions(Prelude);
		}

		[NotNull]
		public TPreludeBuilder Prelude { get; }
		[NotNull]
		public List<AstBuilderSupportingErrors<TLang>> Body { get; } = new List<AstBuilderSupportingErrors<TLang>>();

		[NotNull]
		public TBlockBuilder WithBody([NotNull] Action<TBodyBuilder> bodyOptions)
		{
			bodyOptions(CreateBodyBuilder());
			return (TBlockBuilder) this;
		}

		public abstract class PreludeBuilderBase : AstBuilderSupportingErrors<TLang>
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

			protected abstract TBlockBuilder CreateBlockBuilder(string prelude, Action<TPreludeBuilder> preludeOptions);

			[NotNull]
			protected TBuilder _AddToBody<TBuilder>([NotNull] TBuilder result) where TBuilder : AstBuilderSupportingErrors<TLang>
			{
				_self.Body.Add(result);
				return result;
			}

			[NotNull] protected readonly TBlockBuilder _self;
		}

		protected abstract TBodyBuilder CreateBodyBuilder();

		protected void _BuildBodyInto(List<TLang> destination)
		{
			foreach (var builder in Body) { builder.BuildInto(destination); }
		}
	}
}
