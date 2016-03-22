using System.Collections.Generic;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using Gibberish.Parsing;
using Gibberish.Tests.ZzTestHelpers;
using JetBrains.Annotations;
using NUnit.Framework;

namespace Gibberish.Tests.RecognizeBlockSyntax
{
	[TestFixture]
	public class InterpretValidStatementsAndBlocks
	{
		[Test]
		public void should_recognize_a_non_indented_statement()
		{
			var input = "arbitrary statement\r\n";
			var subject = new RecognizeBlocks();
			var result = subject.GetMatch(input, subject.Statement);
			result.Should()
				.BeRecognizedAs(BasicAst.Statement("arbitrary statement"));
		}
	}

	public static class BasicAst
	{
		public static StatementBuilder Statement([NotNull] string content)
		{
			return new StatementBuilder(content);
		}

		public class StatementBuilder : Builder
		{
			public StatementBuilder(string content)
			{
				Content = content;
			}

			[NotNull]
			public string Content { get; set; }
			[NotNull]
			public IEnumerable<ParseError> Errors { get; set; } = Recognition.NoErrors;

			internal override void Build(List<BareStatement> list)
			{
				list.Add(new BareStatement(Content, Errors));
			}
		}

		public abstract class Builder
		{
			[NotNull]
			public List<BareStatement> Build()
			{
				var statements = new List<BareStatement>();
				Build(statements);
				return statements;
			}

			internal abstract void Build([NotNull] List<BareStatement> list);
		}
	}
}
