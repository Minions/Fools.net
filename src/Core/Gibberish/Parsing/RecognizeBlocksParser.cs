using System;
using System.Collections.Generic;
using System.Linq;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Gibberish.Parsing
{
	partial class RecognizeBlocks
	{
		[NotNull]
		public IEnumerable<LanguageConstruct> ParseWholeFile([NotNull] string input)
		{
			var result = GetMatch(input, WholeFile);
			return result.Results;
			//return ParseWholeFileNewImpl(input);
		}

		[NotNull]
		private IEnumerable<LanguageConstruct> ParseWholeFileNewImpl([NotNull] string input)
		{
			var hasNewlineatEndOfFile = false;
			if (input.EndsWith(CRLF))
			{
				hasNewlineatEndOfFile = true;
				input = input.Substring(0, input.Length - 2);
			}
			else if (input.EndsWith(LF) || input.EndsWith(CR))
			{
				hasNewlineatEndOfFile = true;
				input = input.Substring(0, input.Length - 1);
			}
			var result = input.Split(
				new[]
				{
					CRLF,
					LF,
					CR
				},
				StringSplitOptions.None)
				.Select(_InterpretLine)
				.ToList();
			if (!hasNewlineatEndOfFile) { result[result.Count - 1].Errors.Add(ParseError.MissingNewlineAtEndOfFile()); }
			return result;
		}

		[NotNull]
		private LanguageConstruct _InterpretLine([NotNull] string line)
		{
			var content = line.TrimStart('\t');
			var indentationDepth = line.Length - content.Length;
			if (content.Contains(":"))
			{
				var parts = content.Split(
					new[]
					{
						':'
					},
					2);
				return _ExtractPreludeAndErrors(indentationDepth, parts[0], parts[1], CRLF);
			}
			return _ExtractStatementAndErrors(indentationDepth, content, CRLF);
		}

		private const string CRLF = "\r\n";
		private const string CR = "\r";
		private const string LF = "\n";
	}
}
