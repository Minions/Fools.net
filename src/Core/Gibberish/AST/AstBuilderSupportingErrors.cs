using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	public abstract class AstBuilderSupportingErrors<TConstruct> : AstBuilder<TConstruct>
	{
		[NotNull]
		public List<ParseError> Errors { get; } = ParseError.NoErrors.ToList();

		[NotNull]
		public AstBuilderSupportingErrors<TConstruct> WithError([NotNull] ParseError error)
		{
			Errors.Add(error);
			return this;
		}
	}
}
