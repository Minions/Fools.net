using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class Recognition
	{
		private Recognition(BareStatement[] statements, IEnumerable<ParseError> unattachedErrors)
		{
			Statements = statements;
			UnattachedErrors = unattachedErrors;
		}

		[NotNull] public static readonly IEnumerable<ParseError> NoErrors = Enumerable.Empty<ParseError>();
		[NotNull] public static readonly BareStatement[] NoStatements = {};
		[NotNull] public static readonly Recognition Empty = new Recognition(NoStatements, NoErrors);

		[NotNull]
		public BareStatement[] Statements { get; }
		[NotNull]
		public IEnumerable<ParseError> UnattachedErrors { get; }

		public static Recognition With(BareStatement statement)
		{
			return new Recognition(
				new[]
				{
					statement
				},
				NoErrors);
		}

		public static Recognition WithUnattachedErrors(IEnumerable<ParseError> errors)
		{
			return new Recognition(NoStatements, errors);
		}
	}
}
