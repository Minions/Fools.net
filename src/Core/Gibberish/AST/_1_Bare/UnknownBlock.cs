using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class UnknownBlock : LanguageConstruct
	{
		public UnknownBlock([NotNull] UnknownPrelude prelude, [NotNull] IEnumerable<LanguageConstruct> body, IEnumerable<ParseError> errors) : base(errors)
		{
			Prelude = prelude;
			Body = body.ToArray();
		}

		[NotNull]
		public UnknownPrelude Prelude { get; }
		[NotNull]
		public LanguageConstruct[] Body { get; }
	}
}
