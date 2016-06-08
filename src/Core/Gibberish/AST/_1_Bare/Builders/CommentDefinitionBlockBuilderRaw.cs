using System;
using System.Collections.Generic;

namespace Gibberish.AST._1_Bare.Builders
{
	public class CommentDefinitionBlockBuilderRaw : CommentDefinitionBlockBuilder
	{
		public CommentDefinitionBlockBuilderRaw(int commentId, Action<CommentDefinitionBlockPreludeBuilder> preludeOptions) : base(commentId, preludeOptions) {}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			Prelude.BuildInto(destination);
			foreach (var line in Body) { line.BuildInto(destination); }
		}
	}
}
