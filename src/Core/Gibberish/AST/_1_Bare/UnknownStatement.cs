using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class UnknownStatement : LanguageConstruct
	{
		public UnknownStatement(string content, IEnumerable<ParseError> errors) : base(errors)
		{
			Content = content;
		}

		[NotNull]
		public string Content { get; }
	}
}
