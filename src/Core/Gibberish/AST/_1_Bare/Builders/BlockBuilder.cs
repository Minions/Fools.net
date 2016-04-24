using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class BlockBuilder : BlockBuilderBase<BlockBuilder, BlockBuilder.PreludeBuilder, BlockBuilder.BodyBuilder, LanguageConstruct>
	{
		public BlockBuilder([NotNull] string prelude, [NotNull] Action<PreludeBuilder> preludeOptions) : base(preludeOptions, new PreludeBuilder(prelude)) {}

		public class PreludeBuilder : PreludeBuilderBase
		{
			public PreludeBuilder([NotNull] string content) : base(content) {}

			internal override void Build(List<LanguageConstruct> destination)
			{
				destination.Add(new UnknownPrelude(PossiblySpecified<int>.Unspecifed, Content, Comments, Errors));
			}
		}

		public class BodyBuilder : BodyBuilderBase
		{
			public BodyBuilder([NotNull] BlockBuilder self) : base(self) {}

			[NotNull]
			public StatementBuilder AddStatement([NotNull] string content)
			{
				var statement = new StatementBuilder(content, PossiblySpecified<int>.Unspecifed);
				_self.Body.Add(statement);
				return statement;
			}

			[NotNull]
			public BlankLineBuilder AddBlankLine()
			{
				return _AddToBody(new BlankLineBuilder(PossiblySpecified<int>.Unspecifed));
			}

			protected override BlockBuilder CreateBlockBuilder(string prelude, Action<PreludeBuilder> preludeOptions)
			{
				return new BlockBuilder(prelude, preludeOptions);
			}
		}

		internal override void Build(List<LanguageConstruct> destination)
		{
			var bodyConstructs = new List<LanguageConstruct>();
			var prelude = new List<LanguageConstruct>();
			Prelude.Build(prelude);
			foreach (var builder in Body) { builder.Build(bodyConstructs); }
			destination.Add(
				new UnknownBlock(
					prelude.Cast<UnknownPrelude>()
						.Single(),
					bodyConstructs,
					ParseError.NoErrors));
		}

		protected override BodyBuilder CreateBodyBuilder()
		{
			return new BodyBuilder(this);
		}
	}
}
