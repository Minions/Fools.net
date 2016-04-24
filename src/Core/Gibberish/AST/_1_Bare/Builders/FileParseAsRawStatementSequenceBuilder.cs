using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare.Builders
{
	public class FileParseAsRawStatementSequenceBuilder : AstBuilder<LanguageConstruct>
	{
		public FileParseAsRawStatementSequenceBuilder(Action<FileParseAsRawStatementSequenceBuilder> content)
		{
			content(this);
		}

		[NotNull]
		public BlankLineBuilder BlankLine(int indentationDepth)
		{
			return _Remember(new BlankLineBuilder(PossiblySpecified<int>.WithValue(indentationDepth)));
		}

		public StatementBuilder Statement(string content)
		{
			return _Remember(new StatementBuilder(content, PossiblySpecified<int>.WithValue(0)));
		}

		public RawBlockBuilder RawBlock(string prelude, Action<RawBlockBuilder.PreludeBuilder> preludeOptions)
		{
			return _Remember(new RawBlockBuilder(prelude, preludeOptions, 0));
		}

		public RawBlockBuilder RawBlock(string prelude)
		{
			return RawBlock(prelude, x => { });
		}

		public CommentBuilder CommentDefinition(int commentId, string content)
		{
			return _Remember(new CommentBuilder(commentId, content));
		}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			foreach (var builder in _contents) { builder.BuildInto(destination); }
		}

		private TBuilder _Remember<TBuilder>(TBuilder line) where TBuilder : AstBuilder<LanguageConstruct>
		{
			_contents.Add(line);
			return line;
		}

		[NotNull] private readonly List<AstBuilder<LanguageConstruct>> _contents = new List<AstBuilder<LanguageConstruct>>();
	}
}
