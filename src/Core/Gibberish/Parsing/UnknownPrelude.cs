using System.Collections.Generic;
using System.Linq;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Gibberish.Parsing
{
	public class UnknownPrelude : LanguageConstruct
	{
		public UnknownPrelude([NotNull] string content, [NotNull] IEnumerable<ParseError> errors)
		{
			Content = content;
			Errors = errors.ToArray();
		}

		[NotNull]
		public string Content { get; }
		[NotNull]
		public ParseError[] Errors { get; set; }
	}
}
