using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class RawBlockBuilder : AstBuilder<LanguageConstruct>
	{
		public RawBlockBuilder([NotNull] string prelude, [NotNull] Action<PreludeBuilder> func, int indentationDepth)
		{
			IndentationDepth = indentationDepth;
			Prelude = new PreludeBuilder(prelude, indentationDepth);
			func(Prelude);
		}

		public int IndentationDepth { get; }

		[NotNull]
		public PreludeBuilder Prelude { get; }

		[NotNull]
		public RawBlockBuilder WithBody([NotNull] Action<BodyBuilder> bodyOptions)
		{
			bodyOptions(new BodyBuilder(this));
			return this;
		}

		public class PreludeBuilder : AstBuilder<LanguageConstruct>
		{
			public PreludeBuilder([NotNull] string content, int indentationDepth)
			{
				Content = content;
				IndentationDepth = indentationDepth;
			}

			[NotNull]
			public string Content { get; }

			[NotNull]
			public List<int> Comments { get; } = new List<int>();

			[NotNull]
			public PreludeBuilder WithCommentRefs([NotNull] params int[] indices)
			{
				Comments.AddRange(indices);
				return this;
			}

			public int IndentationDepth { get; }

			internal override void Build(List<LanguageConstruct> destination)
			{
				destination.Add(new UnknownPrelude(IndentationDepth, Content, Comments, Errors));
			}
		}

		public class BodyBuilder
		{
			public BodyBuilder([NotNull] RawBlockBuilder self)
			{
				_self = self;
			}

			[NotNull]
			public StatementBuilder AddStatement([NotNull] string content)
			{
				var statement = new StatementBuilder(content, _self.IndentationDepth + 1);
				_self.Body.Add(statement);
				return statement;
			}

			[NotNull]
			public RawBlockBuilder AddBlock([NotNull] string prelude)
			{
				return AddBlock(prelude, _ => { });
			}

			[NotNull]
			private RawBlockBuilder AddBlock([NotNull] string prelude, [NotNull] Action<PreludeBuilder> preludeOptions)
			{
				var result = new RawBlockBuilder(prelude, preludeOptions, _self.IndentationDepth + 1);
				_self.Body.Add(result);
				return result;
			}

			[NotNull] private readonly RawBlockBuilder _self;
		}

		[NotNull]
		public List<AstBuilder<LanguageConstruct>> Body { get; } = new List<AstBuilder<LanguageConstruct>>();

		internal override void Build(List<LanguageConstruct> destination)
		{
			Prelude.Build(destination);
			foreach (var builder in Body) { builder.Build(destination); }
		}
	}
}
