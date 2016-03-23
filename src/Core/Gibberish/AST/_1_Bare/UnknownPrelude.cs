using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class UnknownPrelude : LanguageConstruct
	{
		public UnknownPrelude(int indentationDepth, [NotNull] string content, IEnumerable<int> comments, [NotNull] IEnumerable<ParseError> errors) : base(errors)
		{
			IndentationDepth = indentationDepth;
			Content = content;
			Comments = comments.ToArray();
		}

		public int IndentationDepth { get; }
		[NotNull]
		public string Content { get; }
		[NotNull]
		public int[] Comments { get; }
	}
}
