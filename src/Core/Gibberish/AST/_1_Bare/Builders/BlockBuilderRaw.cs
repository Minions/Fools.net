using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class BlockBuilderRaw : BlockBuilder
	{
		public BlockBuilderRaw([NotNull] string prelude, [NotNull] Action<PreludeBuilder> preludeOptions, int indentationDepth)
			: base(preludeOptions, new PreludeBuilderImpl(prelude, indentationDepth))
		{
			IndentationDepth = indentationDepth;
		}

		private int IndentationDepth { get; }

		private class PreludeBuilderImpl : PreludeBuilder
		{
			public PreludeBuilderImpl([NotNull] string content, int indentationDepth) : base(content)
			{
				IndentationDepth = PossiblySpecified<int>.WithValue(indentationDepth);
			}

			public override PossiblySpecified<int> IndentationDepth { get; }
		}

		private class BodyBuilderImpl : BodyBuilder
		{
			public BodyBuilderImpl([NotNull] BlockBuilderRaw self) : base(self) {}

			public override PossiblySpecified<int> IndentationDepth => PossiblySpecified<int>.WithValue(((BlockBuilderRaw) _self).IndentationDepth + 1);

			protected override BlockBuilder CreateBlockBuilder(string prelude, Action<PreludeBuilder> preludeOptions)
			{
				return new BlockBuilderRaw(prelude, preludeOptions, IndentationDepth.Value);
			}
		}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			Prelude.BuildInto(destination);
			_BuildBodyInto(destination);
		}

		protected override BodyBuilder _CreateBodyBuilder()
		{
			return new BodyBuilderImpl(this);
		}
	}
}
