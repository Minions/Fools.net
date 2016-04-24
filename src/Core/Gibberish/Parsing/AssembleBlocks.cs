using System.Collections.Generic;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Gibberish.Parsing
{
	public class AssembleBlocks
	{
		[NotNull]
		public List<LanguageConstruct> Transform([NotNull] List<LanguageConstruct> source)
		{
			return _CollectBodyAtLevel(new SourceData(source), 0);
		}

		private static List<LanguageConstruct> _CollectBodyAtLevel(SourceData sourceData, int level)
		{
			var result = new List<LanguageConstruct>();
			while (sourceData.HasMore)
			{
				var line = sourceData.Current;
				if (line.GetType() == typeof (UnknownStatement))
				{
					var unknownStatement = (UnknownStatement) line;
					if (unknownStatement.IndentationDepth.Value < level) { return result; }
					if (unknownStatement.IndentationDepth.Value > level) { unknownStatement.Errors.Add(ParseError.IncorrectIndentation(level, unknownStatement.IndentationDepth.Value)); }
					unknownStatement.StartsParagraph = PossiblySpecified<bool>.WithValue(sourceData.NextItemStartsParagraph);
					sourceData.NextItemStartsParagraph = false;
					result.Add(unknownStatement);
					sourceData.Advance();
				}
				else if (line.GetType() == typeof (BlankLine))
				{
					sourceData.NextItemStartsParagraph = true;
					sourceData.Advance();
				}
				else if (line.GetType() == typeof (CommentDefinition))
				{
					var commentDefinition = (CommentDefinition) line;
					if (0 == level)
					{
						commentDefinition.StartsParagraph = PossiblySpecified<bool>.WithValue(!sourceData.HaveStartedCommentDefinitions);
						result.Add(commentDefinition);
						sourceData.Advance();
						sourceData.HaveStartedCommentDefinitions = true;
					}
					else
					{ return result; }
				}
				else if (line.GetType() == typeof (UnknownPrelude))
				{
					var prelude = (UnknownPrelude) line;
					if (prelude.IndentationDepth.Value == level)
					{
						sourceData.Advance();
						var startsParagraph = sourceData.NextItemStartsParagraph;
						sourceData.NextItemStartsParagraph = false;
						var bodyContents = _CollectBodyAtLevel(sourceData, level + 1);
						result.Add(new UnknownBlock(startsParagraph, prelude, bodyContents, ParseError.NoErrors));
					}
					else
					{ return result; }
				}
				else
				{ return result; }
			}
			return result;
		}

		private class SourceData
		{
			public SourceData([NotNull] List<LanguageConstruct> source)
			{
				_source = source.GetEnumerator();
				HasMore = _source.MoveNext();
			}

			[NotNull]
			public LanguageConstruct Current => _source.Current;
			public bool NextItemStartsParagraph;
			public bool HaveStartedCommentDefinitions;
			public bool HasMore;

			public void Advance()
			{
				if (HasMore) { HasMore = _source.MoveNext(); }
			}

			private List<LanguageConstruct>.Enumerator _source;
		}
	}
}
