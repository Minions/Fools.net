using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class RawBlockBuilder : BlockBuilderBase<RawBlockBuilder, RawBlockBuilder.PreludeBuilder, RawBlockBuilder.BodyBuilder, LanguageConstruct>
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

			[NotNull]
			public StatementBuilder AddStatement([NotNull] string content)
			{
				return _AddToBody(new StatementBuilder(content, PossiblySpecified<int>.WithValue(_self.IndentationDepth + 1)));
			}

			[NotNull]
			public AstBuilder<LanguageConstruct> AddBlankLine()
			{
				return _AddToBody(new BlankLineBuilder(PossiblySpecified<int>.WithValue(_self.IndentationDepth)));
			}

			protected override RawBlockBuilder CreateBlockBuilder(string prelude, Action<PreludeBuilder> preludeOptions)
			{
				return new RawBlockBuilder(prelude, preludeOptions, _self.IndentationDepth + 1);
			}
		}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			Prelude.BuildInto(destination);
			foreach (var builder in Body) { builder.BuildInto(destination); }
		}

		protected override BodyBuilder CreateBodyBuilder()
		{
			return new BodyBuilder(this);
		}
	}
}
