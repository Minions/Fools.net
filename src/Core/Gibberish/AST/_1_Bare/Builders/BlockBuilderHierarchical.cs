using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class BlockBuilderHierarchical : BlockBuilder
	{
		public BlockBuilderHierarchical([NotNull] string prelude, [NotNull] Action<PreludeBuilder> preludeOptions) : base(preludeOptions, new PreludeBuilderImpl(prelude)) {}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			var prelude = new List<LanguageConstruct>();
			Prelude.BuildInto(prelude);
			var bodyConstructs = new List<LanguageConstruct>();
			_BuildBodyInto(bodyConstructs);
			destination.Add(new UnknownBlock(StartsParagraph, (UnknownPrelude) prelude.Single(), bodyConstructs, Errors));
		}

		private class PreludeBuilderImpl : PreludeBuilder
		{
			public PreludeBuilderImpl([NotNull] string content) : base(content) {}

			public override PossiblySpecified<int> IndentationDepth { get; } = PossiblySpecified<int>.Unspecifed;
		}

		protected override BodyBuilder _CreateBodyBuilder()
		{
			return new BodyBuilderImpl(this);
		}

		private class BodyBuilderImpl : BodyBuilder
		{
			public BodyBuilderImpl([NotNull] BlockBuilderHierarchical self) : base(self) {}

			public override PossiblySpecified<int> IndentationDepth => PossiblySpecified<int>.Unspecifed;

			protected override BlockBuilder CreateBlockBuilder(string prelude, Action<PreludeBuilder> preludeOptions)
			{
				return new BlockBuilderHierarchical(prelude, preludeOptions);
			}
		}
	}
}
