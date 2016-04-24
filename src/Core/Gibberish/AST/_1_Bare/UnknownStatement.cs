using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class UnknownStatement : LanguageConstruct
	{
		public UnknownStatement(PossiblySpecified<int> indentationDepth, string content, IEnumerable<int> comments, IEnumerable<ParseError> errors) : base(errors)
		{
			IndentationDepth = indentationDepth;
			Content = content;
			Comments = comments.ToArray();
		}

		public PossiblySpecified<int> IndentationDepth { get; }
		[NotNull]
		public string Content { get; }
		[NotNull]
		public int[] Comments { get; }
	}
}
