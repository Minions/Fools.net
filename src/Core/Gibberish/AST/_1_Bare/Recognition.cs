using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class Recognition
	{
		private Recognition(LanguageConstruct[] items, IEnumerable<ParseError> unattachedErrors)
		{
			Items = items;
			UnattachedErrors = unattachedErrors;
		}

		[NotNull] public static readonly IEnumerable<ParseError> NoErrors = Enumerable.Empty<ParseError>();
		[NotNull] public static readonly LanguageConstruct[] NoStatements = {};
		[NotNull] public static readonly Recognition Empty = new Recognition(NoStatements, NoErrors);

		[NotNull]
		public LanguageConstruct[] Items { get; }
		[NotNull]
		public IEnumerable<ParseError> UnattachedErrors { get; }

		public static Recognition With(LanguageConstruct statement)
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

		public static Recognition Merge(IEnumerable<Recognition> items)
		{
			return new Recognition(
				items.SelectMany(r => r.Items)
					.ToArray(),
				NoErrors);
		}
	}
}
