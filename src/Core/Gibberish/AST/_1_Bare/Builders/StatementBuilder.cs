using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class StatementBuilder : AstBuilderSupportingErrors<LanguageConstruct>
	{
		public StatementBuilder([NotNull] string content, PossiblySpecified<int> indentationDepth)
		{
			Content = content;
			IndentationDepth = indentationDepth;
		}

		[NotNull]
		public string Content { get; }

		public bool StartsParagraph { get; private set; }
		[NotNull]
		public List<int> Comments { get; } = new List<int>();

		public PossiblySpecified<int> IndentationDepth { get; }

		[NotNull]
		public StatementBuilder WithCommentRefs([NotNull] params int[] indices)
		{
			Comments.AddRange(indices);
			return this;
		}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			destination.Add(new UnknownStatement(PossiblySpecified<bool>.WithValue(StartsParagraph), IndentationDepth, Content, Comments, Errors));
		}

		public StatementBuilder ThatStartsNewParagraph()
		{
			StartsParagraph = true;
			return this;
		}
	}
}
