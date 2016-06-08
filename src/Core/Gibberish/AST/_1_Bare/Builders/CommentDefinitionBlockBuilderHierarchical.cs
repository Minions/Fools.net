using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class CommentDefinitionBlockBuilderHierarchical : CommentDefinitionBlockBuilder
	{
		public CommentDefinitionBlockBuilderHierarchical(int commentId, [NotNull] Action<CommentDefinitionBlockPreludeBuilder> preludeOptions) : base(commentId, preludeOptions) {}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			var body = new List<LanguageConstruct>();
			Body.ForEach(b => b.BuildInto(body));
			destination.Add(
				new CommentDefinitionBlock(
					PossiblySpecified<bool>.WithValue(StartsParagraph),
					(CommentDefinitionBlockPrelude) Prelude.Build()
						.Single(),
					body.Cast<CommentDefinitionBlockStatement>(),
					Errors));
		}
	}
}
