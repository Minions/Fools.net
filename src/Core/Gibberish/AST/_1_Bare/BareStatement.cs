using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class BareStatement
	{
		[NotNull]
		public string Content { get; }
		[NotNull]
		public IEnumerable<ParseError> Errors { get; }

		public BareStatement(string content, IEnumerable<ParseError> errors)
		{
			Content = content;
			Errors = errors;
		}
	}
}
