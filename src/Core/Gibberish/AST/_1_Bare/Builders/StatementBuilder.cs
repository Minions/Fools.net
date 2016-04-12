using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class StatementBuilder : AstBuilder<LanguageConstruct>
	{
		public StatementBuilder([NotNull] string content, int indentationDepth)
		{
			Content = content;
			IndentationDepth = indentationDepth;
		}

		[NotNull]
		public string Content { get; }

		[NotNull]
		public List<int> Comments { get; } = new List<int>();

		public int IndentationDepth { get; }

		[NotNull]
		public StatementBuilder WithCommentRefs([NotNull] params int[] indices)
		{
			Comments.AddRange(indices);
			return this;
		}

		internal override void Build(List<LanguageConstruct> destination)
		{
			destination.Add(new UnknownStatement(IndentationDepth, Content, Comments, Errors));
		}
	}
}
