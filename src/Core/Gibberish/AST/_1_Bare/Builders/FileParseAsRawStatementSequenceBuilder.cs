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
			var line = new BlankLineBuilder(PossiblySpecified<int>.WithValue(indentationDepth));
			_contents.Add(line);
			return line;
		}

		public override void BuildInto(List<LanguageConstruct> destination)
		{
			foreach (var builder in _contents) { builder.BuildInto(destination); }
		}

		[NotNull] private readonly List<AstBuilder<LanguageConstruct>> _contents = new List<AstBuilder<LanguageConstruct>>();

		public StatementBuilder Statement(string content)
		{
			return new StatementBuilder(content, PossiblySpecified<int>.WithValue(0));
		}

		public RawBlockBuilder RawBlock(string prelude, Action<RawBlockBuilder.PreludeBuilder> preludeOptions)
		{
			return new RawBlockBuilder(prelude, preludeOptions, 0);
		}

		public RawBlockBuilder RawBlock(string prelude)
		{
			return RawBlock(prelude, x => { });
		}

		public CommentBuilder CommentDefinition(int commentId, string content)
		{
			return new CommentBuilder(commentId, content);
		}
	}
}