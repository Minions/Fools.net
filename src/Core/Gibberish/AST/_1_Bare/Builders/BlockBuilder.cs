using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class BlockBuilder : BlockBuilderBase<BlockBuilder, BlockBuilder.PreludeBuilder, BlockBuilder.BodyBuilder, LanguageConstruct>
	{
		public BlockBuilder([NotNull] string prelude, [NotNull] Action<PreludeBuilder> preludeOptions, int indentationDepth)
			: base(preludeOptions, new PreludeBuilder(prelude, indentationDepth))
		{
			IndentationDepth = indentationDepth;
		}

		public int IndentationDepth { get; }

		public class PreludeBuilder : PreludeBuilderBase
		{
			public PreludeBuilder([NotNull] string content, int indentationDepth) : base(content)
			{
				IndentationDepth = indentationDepth;
			}

			public int IndentationDepth { get; }

			internal override void Build(List<LanguageConstruct> destination)
			{
				destination.Add(new UnknownPrelude(IndentationDepth, Content, Comments, Errors));
			}
		}

		public class BodyBuilder : BodyBuilderBase
		{
			public BodyBuilder([NotNull] BlockBuilder self) : base(self) {}

			[NotNull]
			public StatementBuilder AddStatement([NotNull] string content)
			{
				var statement = new StatementBuilder(content, _self.IndentationDepth + 1);
				_self.Body.Add(statement);
				return statement;
			}

			protected override BlockBuilder CreateBlockBuilder(string prelude, Action<PreludeBuilder> preludeOptions)
			{
				return new BlockBuilder(prelude, preludeOptions, _self.IndentationDepth + 1);
			}
		}

		internal override void Build(List<LanguageConstruct> destination)
		{
			Prelude.Build(destination);
			foreach (var builder in Body) { builder.Build(destination); }
		}

		protected override BodyBuilder CreateBodyBuilder()
		{
			return new BodyBuilder(this);
		}
	}
}