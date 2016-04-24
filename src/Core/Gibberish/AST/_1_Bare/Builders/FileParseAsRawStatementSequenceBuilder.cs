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
	}
}