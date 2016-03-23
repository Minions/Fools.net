using System.Collections.Generic;
using System.Linq;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Gibberish.Parsing
{
	public class UnknownPrelude : LanguageConstruct
	{
		public UnknownPrelude([NotNull] string content, IEnumerable<int> comments, [NotNull] IEnumerable<ParseError> errors) : base(errors)
		{
			Content = content;
			Comments = comments.ToArray();
		}

		[NotNull]
		public string Content { get; }
		[NotNull]
		public int[] Comments { get; }
	}
}
