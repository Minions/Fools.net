using System.Collections.Generic;
using Gibberish.AST;
using Gibberish.AST._1_Bare;

namespace Gibberish.Parsing
{
	public class AssembleBlocks
	{
		private static bool _nextItemStartsParagraph;

		public List<LanguageConstruct> Transform(List<LanguageConstruct> source)
		{
			return _CollectBodyAtLevel(source, 0);
		}

		private static List<LanguageConstruct> _CollectBodyAtLevel(List<LanguageConstruct> source, int level)
		{
			var result = new List<LanguageConstruct>();
			while (source.Count != 0)
			{
				var line = source[0];
				if (line.GetType() == typeof (UnknownStatement))
				{
					var unknownStatement = (UnknownStatement) line;
					if (unknownStatement.IndentationDepth.Value == level)
					{
						unknownStatement.StartsParagraph = PossiblySpecified<bool>.WithValue(_nextItemStartsParagraph);
						_nextItemStartsParagraph = false;
						result.Add(unknownStatement);
						source.RemoveAt(0);
					}
					else
					{ return result; }
				}
				else if (line.GetType() == typeof (BlankLine))
				{
					_nextItemStartsParagraph = true;
					source.RemoveAt(0);
				}
				else if (line.GetType() == typeof (CommentDefinition))
				{
					var commentDefinition = (CommentDefinition) line;
					if (0 == level)
					{
						result.Add(commentDefinition);
						source.RemoveAt(0);
					}
					else
					{ return result; }
				}
				else if (line.GetType() == typeof (UnknownPrelude))
				{
					var prelude = (UnknownPrelude) line;
					if (prelude.IndentationDepth.Value == level)
					{
						source.RemoveAt(0);
						var startsParagraph = _nextItemStartsParagraph;
						_nextItemStartsParagraph = false;
						var bodyContents = _CollectBodyAtLevel(source, level + 1);
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
