using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class UnknownBlock : LanguageConstruct
	{
		public UnknownBlock([NotNull] string prelude, [NotNull] IEnumerable<LanguageConstruct> body)
		{
			Prelude = prelude;
			Body = body.ToArray();
		}

		[NotNull]
		public string Prelude { get; }
		[NotNull]
		public LanguageConstruct[] Body { get; }
	}
}
