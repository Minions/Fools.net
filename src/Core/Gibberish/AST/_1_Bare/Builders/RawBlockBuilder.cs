using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class RawBlockBuilder : BlockBuilderBase<RawBlockBuilder.PreludeBuilder, RawBlockBuilder.BodyBuilder>
	{
		public RawBlockBuilder([NotNull] string prelude, [NotNull] Action<PreludeBuilder> preludeOptions, int indentationDepth)
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

			public override void BuildInto(List<LanguageConstruct> destination)
			{
				destination.Add(new UnknownPrelude(PossiblySpecified<int>.WithValue(IndentationDepth), Content, Comments, Errors));
			}
		}

		public class BodyBuilder : BodyBuilderBase
		{
			public BodyBuilder([NotNull] RawBlockBuilder self) : base(self) {}

			public override PossiblySpecified<int> IndentationDepth => PossiblySpecified<int>.WithValue(((RawBlockBuilder) _self).IndentationDepth + 1);

			protected override BlockBuilderBase<PreludeBuilder, BodyBuilder> CreateBlockBuilder(string prelude, Action<PreludeBuilder> preludeOptions)
			{
				return new RawBlockBuilder(prelude, preludeOptions, IndentationDepth.Value);
			}
		}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			Prelude.BuildInto(destination);
			_BuildBodyInto(destination);
		}

		protected override BodyBuilder CreateBodyBuilder()
		{
			return new BodyBuilder(this);
		}
	}
}
