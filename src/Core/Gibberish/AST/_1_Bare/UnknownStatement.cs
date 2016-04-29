using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class UnknownStatement : LanguageConstruct
	{
		public UnknownStatement(PossiblySpecified<bool> startsParagraph, PossiblySpecified<int> indentationDepth, string content, IEnumerable<int> comments,
			IEnumerable<ParseError> errors) : base(errors)
		{
			StartsParagraph = startsParagraph;
			IndentationDepth = indentationDepth;
			Content = content;
			Comments = comments.ToArray();
		}

		public PossiblySpecified<bool> StartsParagraph { get; set; }
		public PossiblySpecified<int> IndentationDepth { get; set; }
		[NotNull]
		public string Content { get; }
		[NotNull]
		public int[] Comments { get; }
	}
}
