using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class BareStatement
	{
		public BareStatement(string content, IEnumerable<ParseError> errors)
		{
			Content = content;
			Errors = errors.ToArray();
		}

		[NotNull]
		public string Content { get; }
		[NotNull]
		public ParseError[] Errors { get; }
	}
}
