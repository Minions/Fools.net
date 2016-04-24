using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class UnknownPrelude : LanguageConstruct
	{
		public UnknownPrelude(PossiblySpecified<int> indentationDepth, [NotNull] string content, IEnumerable<int> comments, [NotNull] IEnumerable<ParseError> errors) : base(errors)
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
