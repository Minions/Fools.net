using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class UnknownBlock : LanguageConstruct
	{
		public UnknownBlock([NotNull] string prelude, [NotNull] IEnumerable<LanguageConstruct> body, IEnumerable<ParseError> errors)
		{
			Prelude = prelude;
			Errors = errors.ToArray();
			Body = body.ToArray();
		}

		[NotNull]
		public string Prelude { get; }
		[NotNull]
		public ParseError[] Errors { get; }
		[NotNull]
		public LanguageConstruct[] Body { get; }
	}
}
