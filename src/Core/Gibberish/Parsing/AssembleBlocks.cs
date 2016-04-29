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
			var sourceData = new SourceData(source);
			return _CollectBodyAtLevel(new BlockRecognizer(sourceData), 0, sourceData);
		}

		public class SourceData
		{
			public SourceData([NotNull] List<LanguageConstruct> source)
			{
				_source = source.GetEnumerator();
				HasMore = _source.MoveNext();
			}

			[NotNull]
			public LanguageConstruct Current => _source.Current;
			public bool NextItemStartsParagraph { get; private set; }
			public bool HasMore;

			public void ContinueToNextLine(bool nextItemStartsParagraph)
			{
				NextItemStartsParagraph = nextItemStartsParagraph && !_haveStartedCommentDefinitions;
				if (HasMore) { HasMore = _source.MoveNext(); }
			}

			public void HaveSeenAtLeastOneCommentDefinition()
			{
				NextItemStartsParagraph = !_haveStartedCommentDefinitions;
				_haveStartedCommentDefinitions = true;
			}

			public PossiblySpecified<bool> ShouldStartParagraph => PossiblySpecified<bool>.WithValue(NextItemStartsParagraph);
			private bool _haveStartedCommentDefinitions;

			private List<LanguageConstruct>.Enumerator _source;
		}

		private static List<LanguageConstruct> _CollectBodyAtLevel(LanguageConstructVisitor worker, int level, SourceData sourceData)
		{
			var result = new List<LanguageConstruct>();
			while (sourceData.HasMore)
			{
				var line = sourceData.Current;
				if (line.Accept(worker, level, result)) { return result; }
			}
			return result;
		}

		private class BlockRecognizer : LanguageConstructVisitor
		{
			public BlockRecognizer([NotNull] SourceData sourceData)
			{
				_sourceData = sourceData;
			}

			public bool Visit(BlankLine line, int level, List<LanguageConstruct> result)
			{
				_sourceData.ContinueToNextLine(true);
				return false;
			}

			public bool Visit(UnknownStatement statement, int level, List<LanguageConstruct> result)
			{
				if (_requirePerfectIndentation && (statement.IndentationDepth.Value != level)) { return true; }
				_requirePerfectIndentation = false;
				if (statement.IndentationDepth.Value < level) { return true; }
				if (statement.IndentationDepth.Value > level) { statement.Errors.Add(ParseError.IncorrectIndentation(level, statement.IndentationDepth.Value)); }
				statement.StartsParagraph = _sourceData.ShouldStartParagraph;
				result.Add(statement);
				_sourceData.ContinueToNextLine(false);
				return false;
			}

			public bool Visit(CommentDefinition commentDefinition, int level, List<LanguageConstruct> result)
			{
				if (0 != level) { return true; }
				_sourceData.HaveSeenAtLeastOneCommentDefinition();
				commentDefinition.StartsParagraph = _sourceData.ShouldStartParagraph;
				result.Add(commentDefinition);
				_sourceData.ContinueToNextLine(false);
				return false;
			}

			public bool Visit(UnknownPrelude prelude, int level, List<LanguageConstruct> result)
			{
				if (_requirePerfectIndentation && (prelude.IndentationDepth.Value != level)) { return true; }
				_requirePerfectIndentation = false;
				if (prelude.IndentationDepth.Value < level) { return true; }

				var startsParagraph = _sourceData.NextItemStartsParagraph;
				_sourceData.ContinueToNextLine(false);
				_requirePerfectIndentation = true;
				var bodyContents = _CollectBodyAtLevel(this, prelude.IndentationDepth.Value + 1, _sourceData);
				_requirePerfectIndentation = false;

				var errors = new List<ParseError>();
				if (prelude.IndentationDepth.Value > level)
				{
					if (bodyContents.Count != 0) { errors.Add(ParseError.WholeBlockIsIndentedTooFar(level, prelude.IndentationDepth.Value)); }
					else
					{
						errors.Add(ParseError.IncorrectIndentation(level, prelude.IndentationDepth.Value));
						_requirePerfectIndentation = true;
						bodyContents = _CollectBodyAtLevel(this, level + 1, _sourceData);
						_requirePerfectIndentation = false;
						if (bodyContents.Count == 0) { errors.Add(ParseError.MissingBody()); }
					}
				}
				else if (bodyContents.Count == 0) { errors.Add(ParseError.MissingBody()); }

				result.Add(new UnknownBlock(startsParagraph, prelude, bodyContents, errors));
				return false;
			}

			public bool Visit(UnknownBlock block, int level, List<LanguageConstruct> result)
			{
				throw new UnfixableError($"a block somehow made into input data for {typeof (AssembleBlocks).Name}. Found value {block}.");
			}

			[NotNull] private readonly SourceData _sourceData;
			private bool _requirePerfectIndentation;
		}
	}
}
