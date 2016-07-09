using System;
using System.Collections.Generic;
using System.Linq;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using Gibberish.Parsing.LineParsers;
using JetBrains.Annotations;

namespace Gibberish.Parsing
{
	public class RecognizeLines : Transform<string, List<LanguageConstruct>>
	{
		public List<LanguageConstruct> Transform(string input)
		{
			return ParseWholeFile(input).ToList();
		}

		[NotNull]
		public IEnumerable<LanguageConstruct> ParseWholeFile([NotNull] string input)
		{
			var hasNewlineatEndOfFile = _DetectAndHandleNewlineAtFileEnd(ref input);
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

		public void RememberThatHaveFoundCommentSection()
		{
			InCommentSection = true;
		}

		private bool InCommentSection { get; set; }

		private static bool _DetectAndHandleNewlineAtFileEnd([NotNull] ref string input)
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
			return hasNewlineatEndOfFile;
		}

		[NotNull]
		private LanguageConstruct _InterpretLine([NotNull] string line)
		{
			var content = line.TrimStart('\t');
			var indentationDepth = line.Length - content.Length;
			if (ParseCommentDefinitionPreludes.Matches(content)) { return ParseCommentDefinitionPreludes.Interpret(indentationDepth, content, this); }
			if (ParseCommentDefinitions.Matches(content)) { return ParseCommentDefinitions.Interpret(content, this); }
			if (InCommentSection) { return ParseCommentStatements.Interpret(indentationDepth, content); }
			if (ParseBlankLines.Matches(content)) { return ParseBlankLines.Interpret(indentationDepth, content); }
			if (ParsePreludes.Matches(content)) { return ParsePreludes.Interpret(indentationDepth, content); }
			return ParseStatements.Interpret(indentationDepth, content);
		}

		private const string CRLF = CR + LF;
		private const string CR = "\r";
		private const string LF = "\n";
	}
}
