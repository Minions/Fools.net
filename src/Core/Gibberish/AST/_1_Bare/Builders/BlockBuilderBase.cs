using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public abstract class BlockBuilderBase : AstBuilderSupportingErrors<LanguageConstruct>
	{
		protected BlockBuilderBase([NotNull] Action<PreludeBuilderBase> preludeOptions, PreludeBuilderBase preludeBuilder)
		{
			Prelude = preludeBuilder;
			preludeOptions(Prelude);
		}

		[NotNull]
		public PreludeBuilderBase Prelude { get; }
		[NotNull]
		public List<AstBuilderSupportingErrors<LanguageConstruct>> Body { get; } = new List<AstBuilderSupportingErrors<LanguageConstruct>>();
		public bool StartsParagraph { get; private set; }

		[NotNull]
		public BlockBuilderBase WithBody([NotNull] Action<BodyBuilderBase> bodyOptions)
		{
			bodyOptions(CreateBodyBuilder());
			return this;
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
			public PreludeBuilderBase WithCommentRefs([NotNull] params int[] indices)
			{
				Comments.AddRange(indices);
				return this;
			}
		}

		public abstract class BodyBuilderBase
		{
			protected BodyBuilderBase([NotNull] BlockBuilderBase self)
			{
				_self = self;
			}

			[NotNull]
			public BlockBuilderBase AddBlock([NotNull] string prelude)
			{
				return AddBlock(prelude, _ => { });
			}

			[NotNull]
			public BlockBuilderBase AddBlock([NotNull] string prelude, [NotNull] Action<PreludeBuilderBase> preludeOptions)
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

			protected abstract BlockBuilderBase CreateBlockBuilder(string prelude, Action<PreludeBuilderBase> preludeOptions);

			[NotNull]
			private TBuilder _AddToBody<TBuilder>([NotNull] TBuilder result) where TBuilder : AstBuilderSupportingErrors<LanguageConstruct>
			{
				_self.Body.Add(result);
				return result;
			}

			[NotNull] protected readonly BlockBuilderBase _self;
		}

		protected abstract BodyBuilderBase CreateBodyBuilder();

		protected void _BuildBodyInto(List<LanguageConstruct> destination)
		{
			foreach (var builder in Body) { builder.BuildInto(destination); }
		}

		public BlockBuilderBase ThatStartsNewParagraph()
		{
			StartsParagraph = true;
			return this;
		}
	}
}
