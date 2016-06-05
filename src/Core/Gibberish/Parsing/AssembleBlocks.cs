using System.Collections.Generic;
using System.Linq;
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
				NextItemStartsParagraph = nextItemStartsParagraph && !HaveStartedCommentDefinitions;
				if (HasMore) { HasMore = _source.MoveNext(); }
			}

			public void HaveSeenAtLeastOneCommentDefinition()
			{
				NextItemStartsParagraph = !HaveStartedCommentDefinitions;
				HaveStartedCommentDefinitions = true;
			}

			public PossiblySpecified<bool> ShouldStartParagraph => PossiblySpecified<bool>.WithValue(NextItemStartsParagraph);
			public bool HaveStartedCommentDefinitions { get; private set; }

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
				throw new UnfixableError($"a block somehow made it into input data for {typeof (AssembleBlocks).Name}. Found value {block}.");
			}

			public bool Visit(MultilineCommentDefinitionPrelude prelude, int level, List<LanguageConstruct> result)
			{
				if (0 != level) { return true; }
				var errors = new List<ParseError>();
				_sourceData.HaveSeenAtLeastOneCommentDefinition();
				var startsParagraph = _sourceData.ShouldStartParagraph;
				var contents = new List<MultilineCommentDefinitionStatement>();
				_sourceData.ContinueToNextLine(false);
				while (_sourceData.Current is MultilineCommentDefinitionStatement) {
					contents.Add((MultilineCommentDefinitionStatement) _sourceData.Current);
					_sourceData.ContinueToNextLine(false);
				}
				result.Add(new CommentDefinitionBlock(startsParagraph, prelude, contents, errors));
				return false;
			}

			public bool Visit(MultilineCommentDefinitionStatement statement, int level, List<LanguageConstruct> result)
			{
				throw new System.NotImplementedException();
			}

			[NotNull] private readonly SourceData _sourceData;
			private bool _requirePerfectIndentation;
		}
	}
}
