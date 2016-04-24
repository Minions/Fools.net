using System.Collections.Generic;
using Gibberish.AST;
using Gibberish.AST._1_Bare;

namespace Gibberish.Parsing
{
	public class AssembleBlocks
	{
		public List<LanguageConstruct> Transform(List<LanguageConstruct> source)
		{
			return _CollectBodyAtLevel(new _SourceData(source), 0);
		}

		public class _SourceData
		{
			public _SourceData(List<LanguageConstruct> source)
			{
				Source = source;
			}

			public List<LanguageConstruct> Source { get; }
			public bool NextItemStartsParagraph;
			public bool HaveStartedCommentDefinitions;
		}

		private static List<LanguageConstruct> _CollectBodyAtLevel(_SourceData sourceData, int level)
		{
			var result = new List<LanguageConstruct>();
			while (sourceData.Source.Count != 0)
			{
				var line = sourceData.Source[0];
				if (line.GetType() == typeof (UnknownStatement))
				{
					var unknownStatement = (UnknownStatement) line;
					if (unknownStatement.IndentationDepth.Value == level)
					{
						unknownStatement.StartsParagraph = PossiblySpecified<bool>.WithValue(sourceData.NextItemStartsParagraph);
						sourceData.NextItemStartsParagraph = false;
						result.Add(unknownStatement);
						sourceData.Source.RemoveAt(0);
					}
					else
					{ return result; }
				}
				else if (line.GetType() == typeof (BlankLine))
				{
					sourceData.NextItemStartsParagraph = true;
					sourceData.Source.RemoveAt(0);
				}
				else if (line.GetType() == typeof (CommentDefinition))
				{
					var commentDefinition = (CommentDefinition) line;
					if (0 == level)
					{
						commentDefinition.StartsParagraph = PossiblySpecified<bool>.WithValue(!sourceData.HaveStartedCommentDefinitions);
						result.Add(commentDefinition);
						sourceData.Source.RemoveAt(0);
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
						sourceData.Source.RemoveAt(0);
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
	}
}
