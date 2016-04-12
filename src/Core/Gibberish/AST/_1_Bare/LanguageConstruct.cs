using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class LanguageConstruct : FasmOrLanguageConstruct
	{
		protected LanguageConstruct([NotNull] IEnumerable<ParseError> errors)
		{
			Errors = errors.ToArray();
		}

		[NotNull]
		public ParseError[] Errors { get; }
	}
}
