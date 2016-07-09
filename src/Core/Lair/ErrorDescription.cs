using Gibberish.AST;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Lair
{
	internal class ErrorDescription
	{
		public ErrorDescription(int lineNumber, [NotNull] string content, [NotNull] ParseError error, [NotNull] LanguageConstruct line)
		{
			_lineNumber = lineNumber;
			_content = content;
			_error = error;
			_line = line;
		}

		public override string ToString()
		{
			return $"[{_lineNumber}]\t{_content}\r\nError: {_error.message}";
		}

		[NotNull] private readonly string _content;
		[NotNull] private readonly ParseError _error;
		[NotNull] private readonly LanguageConstruct _line;
		private readonly int _lineNumber;
	}
}
