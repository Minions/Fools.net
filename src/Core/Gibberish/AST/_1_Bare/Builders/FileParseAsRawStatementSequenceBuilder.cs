using System;

namespace Gibberish.AST._1_Bare.Builders
{
	public class FileParseAsRawStatementSequenceBuilder : FileParseBuilderBase<RawBlockBuilder, RawBlockBuilder.PreludeBuilder>
	{
		public FileParseAsRawStatementSequenceBuilder(Action<FileParseAsRawStatementSequenceBuilder> content)
		{
			content(this);
		}

		public override BlankLineBuilder BlankLine()
		{
			return _Remember(new BlankLineBuilder(PossiblySpecified<int>.Unspecifed));
		}

		public BlankLineBuilder BlankLine(int indentationDepth)
		{
			return _Remember(new BlankLineBuilder(PossiblySpecified<int>.WithValue(indentationDepth)));
		}

		public override StatementBuilder Statement(string content)
		{
			return _Remember(new StatementBuilder(content, PossiblySpecified<int>.WithValue(0)));
		}

		public override RawBlockBuilder Block(string prelude, Action<RawBlockBuilder.PreludeBuilder> preludeOptions)
		{
			return _Remember(new RawBlockBuilder(prelude, preludeOptions, 0));
		}

		public override RawBlockBuilder Block(string prelude)
		{
			return Block(prelude, x => { });
		}

		public override CommentBuilder CommentDefinition(int commentId, string content)
		{
			return _Remember(new CommentBuilder(commentId, content));
		}
	}
}
