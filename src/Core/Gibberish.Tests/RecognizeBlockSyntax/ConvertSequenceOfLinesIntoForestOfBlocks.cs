using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using Gibberish.AST._1_Bare.Builders;
using Gibberish.Execution;
using Gibberish.Parsing;
using Gibberish.Tests.ZzTestHelpers;
using JetBrains.Annotations;
using NUnit.Framework;

namespace Gibberish.Tests.RecognizeBlockSyntax
{
	[TestFixture]
	public class ConvertSequenceOfLinesIntoForestOfBlocks
	{
		[Test]
		public void ShouldFindBlockwithSimpleStatements()
		{
			Action<FileParseBuilder> code = f => f.Block("outer block")
				.WithBody(
					b =>
					{
						b.AddStatement("outer 1");
						b.AddStatement("outer 2");
					});
			ShouldTransformCodeFromLinesRepresentationIntoTreeRepresentation(code);
		}

		[Test]
		public void BlocksShouldBeNestable()
		{
			Action<FileParseBuilder> code = f => f.Block("outer block")
				.WithBody(
					b =>
					{
						b.AddBlock("nested 1")
							.WithBody(inner => inner.AddStatement("nested 1.1"));
					});
			ShouldTransformCodeFromLinesRepresentationIntoTreeRepresentation(code);
		}

		[Test]
		public void FollowingABlockWithALessNestedStatementEndsTheInnerBlock()
		{
			Action<FileParseBuilder> code = f => f.Block("outer block")
				.WithBody(
					b =>
					{
						b.AddBlock("nested 1")
							.WithBody(inner => inner.AddStatement("nested 1.1"));
						b.AddStatement("outer 1");
					});
			ShouldTransformCodeFromLinesRepresentationIntoTreeRepresentation(code);
		}

		[Test]
		public void FollowingABlockWithALessNestedBlockEndsTheInnerBlockAndStartsANewOne()
		{
			Action<FileParseBuilder> code = f => f.Block("outer block")
				.WithBody(
					b =>
					{
						b.AddBlock("nested 1")
							.WithBody(inner => inner.AddStatement("nested 1.1"));
						b.AddBlock("nested 2")
							.WithBody(inner => inner.AddStatement("nested 2.1"));
					});
			ShouldTransformCodeFromLinesRepresentationIntoTreeRepresentation(code);
		}

		[Test]
		public void IndentingAStatementBeyondCurrentLevelGivesAnError()
		{
			var lines = new List<LanguageConstruct>
			{
				StatementIndented(0, ArbitraryContent),
				StatementIndented(1, ArbitraryContent)
			};
			var tree = BasicAst.BlockTree(
				f =>
				{
					f.Statement(ArbitraryContent);
					f.Statement(ArbitraryContent, 1)
						.WithError(ParseError.IncorrectIndentation(0, 1));
				});
			ShouldTransformLinesIntoTree(lines, tree);
		}

		[Test]
		public void IndentingAPreludeBeyondCurrentLevelGivesAnError()
		{
			var lines = new List<LanguageConstruct>
			{
				StatementIndented(0, ArbitraryContent),
				PreludeIndented(1, ArbitraryContent),
				StatementIndented(2, ArbitraryContent)
			};
			var tree = BasicAst.BlockTree(
				f =>
				{
					f.Statement(ArbitraryContent);
					f.Block(ArbitraryContent)
						.WithBody(b => b.AddStatement(ArbitraryContent))
						.WithError(ParseError.WholeBlockIsIndentedTooFar(0, 1));
				});
			ShouldTransformLinesIntoTree(lines, tree);
		}

		[Test]
		public void IndentingJustThePreludeLineTooFarGivesSpecialError()
		{
			var lines = new List<LanguageConstruct>
			{
				StatementIndented(0, ArbitraryContent),
				PreludeIndented(3, ArbitraryContent),
				StatementIndented(1, ArbitraryContent)
			};
			var tree = BasicAst.BlockTree(
				f =>
				{
					f.Statement(ArbitraryContent);
					f.Block(ArbitraryContent)
						.WithBody(b => b.AddStatement(ArbitraryContent))
						.WithError(ParseError.IncorrectIndentation(0, 3));
				});
			ShouldTransformLinesIntoTree(lines, tree);
		}

		[Test]
		public void IndentingThePreludeLineTooFarWithNoBlockBodyGivesCorrectErrors()
		{
			var lines = new List<LanguageConstruct>
			{
				StatementIndented(0, ArbitraryContent),
				PreludeIndented(3, ArbitraryContent),
				StatementIndented(0, ArbitraryContent)
			};
			var tree = BasicAst.BlockTree(
				f =>
				{
					f.Statement(ArbitraryContent);
					f.Block(ArbitraryContent)
						.WithError(ParseError.IncorrectIndentation(0, 3))
						.WithError(ParseError.MissingBody());
					f.Statement(ArbitraryContent);
				});
			ShouldTransformLinesIntoTree(lines, tree);
		}

		[Test]
		public void BadPreludeIndentFollowedByMoreBadIndentingThatCannotBeInterpretedAnyOtherWayResultsInIndependentErrors()
		{
			var lines = new List<LanguageConstruct>
			{
				StatementIndented(0, ArbitraryContent),
				PreludeIndented(3, ArbitraryContent),
				StatementIndented(2, ArbitraryContent)
			};
			var tree = BasicAst.BlockTree(
				f =>
				{
					f.Statement(ArbitraryContent);
					f.Block(ArbitraryContent)
						.WithError(ParseError.IncorrectIndentation(0, 3))
						.WithError(ParseError.MissingBody());
					f.Statement(ArbitraryContent, 2)
						.WithError(ParseError.IncorrectIndentation(0, 2));
				});
			ShouldTransformLinesIntoTree(lines, tree);
		}

		[Test]
		public void BadPreludeIndentFollowedByMoreBadIndentingInAnotherWayThatCannotBeInterpretedAnyOtherWayResultsInIndependentErrors()
		{
			var lines = new List<LanguageConstruct>
			{
				StatementIndented(0, ArbitraryContent),
				PreludeIndented(3, ArbitraryContent),
				PreludeIndented(5, ArbitraryContent)
			};
			var tree = BasicAst.BlockTree(
				f =>
				{
					f.Statement(ArbitraryContent);
					f.Block(ArbitraryContent)
						.WithError(ParseError.IncorrectIndentation(0, 3))
						.WithError(ParseError.MissingBody());
					f.Block(ArbitraryContent)
						.WithError(ParseError.IncorrectIndentation(0, 5))
						.WithError(ParseError.MissingBody());
				});
			ShouldTransformLinesIntoTree(lines, tree);
		}

		[Test]
		public void PreludeFollowedByNonIndentedStatementGivesError()
		{
			var lines = new List<LanguageConstruct>
			{
				PreludeIndented(0, ArbitraryContent),
				StatementIndented(0, ArbitraryContent)
			};
			var tree = BasicAst.BlockTree(
				f =>
				{
					f.Block(ArbitraryContent)
						.WithError(ParseError.MissingBody());
					f.Statement(ArbitraryContent);
				});
			ShouldTransformLinesIntoTree(lines, tree);
		}

		[Test]
		public void PreludeFollowedCommentDefinitionGivesError()
		{
			var lines = new List<LanguageConstruct>
			{
				PreludeIndented(0, ArbitraryContent),
				Comment(1, ArbitraryContent)
			};
			var tree = BasicAst.BlockTree(
				f =>
				{
					f.Block(ArbitraryContent)
						.WithError(ParseError.MissingBody());
					f.CommentDefinition(1, ArbitraryContent)
						.ThatStartsParagraph();
				});
			ShouldTransformLinesIntoTree(lines, tree);
		}

		[Test]
		public void BlankLinesShouldStartNewParagraphAndMaintainSameBlock()
		{
			var lines = new List<LanguageConstruct>
			{
				PreludeIndented(0, "outer block"),
				StatementIndented(1, "outer 1"),
				BlankLineIndented(0),
				StatementIndented(1, "outer 2")
			};
			var tree = BasicAst.BlockTree(
				f => f.Block("outer block")
					.WithBody(
						b =>
						{
							b.AddStatement("outer 1");
							b.AddStatement("outer 2")
								.ThatStartsNewParagraph();
						}));
			ShouldTransformLinesIntoTree(lines, tree);
		}

		[Test]
		public void MultipleBlankLinesShouldBeTreatedLikeOne()
		{
			var lines = new List<LanguageConstruct>
			{
				StatementIndented(0, "1"),
				BlankLineIndented(0),
				BlankLineIndented(0),
				StatementIndented(0, "2")
			};
			var tree = BasicAst.BlockTree(
				f =>
				{
					f.Statement("1");
					f.Statement("2")
						.ThatStartsNewParagraph();
				});
			ShouldTransformLinesIntoTree(lines, tree);
		}

		[Test]
		public void BlankLinesBeforeBlocksShouldStartNewParagraph()
		{
			var lines = new List<LanguageConstruct>
			{
				StatementIndented(0, "1"),
				BlankLineIndented(0),
				PreludeIndented(0, "outer"),
				StatementIndented(1, "outer 1")
			};
			var tree = BasicAst.BlockTree(
				f =>
				{
					f.Statement("1");
					f.Block("outer")
						.WithBody(b => b.AddStatement("outer 1"))
						.ThatStartsNewParagraph();
				});
			ShouldTransformLinesIntoTree(lines, tree);
		}

		[Test]
		public void CommentDefinitionsShouldTerminatePreceedingBlocksAndAppearAtRootLevelAsNewParagraph()
		{
			Action<FileParseBuilder> code = f =>
			{
				f.Block("outer block")
					.WithBody(b => { b.AddStatement("outer 1"); });
				f.CommentDefinition(3, ArbitraryComment)
					.ThatStartsParagraph();
			};
			ShouldTransformCodeFromLinesRepresentationIntoTreeRepresentation(code);
		}

		[Test]
		public void SubsequentCommentDefsNeverStartNewParagraphsEvenIfThereAreBlankLines()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				BasicAst.SequenceOfRawLines(
					f =>
					{
						f.CommentDefinition(1, ArbitraryComment);
						f.BlankLine();
						f.CommentDefinition(2, ArbitraryComment);
						f.CommentDefinition(3, ArbitraryComment);
					})
					.Build());
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f =>
						{
							f.CommentDefinition(1, ArbitraryComment)
								.ThatStartsParagraph();
							f.CommentDefinition(2, ArbitraryComment);
							f.CommentDefinition(3, ArbitraryComment);
						}));
		}

		[Test]
		public void MultiLineCommentPreludeFollowedByStatementBecomesAMultiLineComment()
		{
			Action<FileParseBuilder> code = f =>
			{
				f.CommentDefinitionBlock(8)
					.WithBody(b => { b.AddStatement("first"); })
					.ThatStartsParagraph();
			};
			ShouldTransformCodeFromLinesRepresentationIntoTreeRepresentation(code);
		}

		private static void ShouldTransformCodeFromLinesRepresentationIntoTreeRepresentation(Action<FileParseBuilder> code)
		{
			ShouldTransformLinesIntoTree(
				BasicAst.SequenceOfRawLines(code)
					.Build(),
				BasicAst.BlockTree(code));
		}

		private static void ShouldTransformLinesIntoTree(List<LanguageConstruct> lines, FileParseBuilder tree)
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(lines);
			result.Should()
				.BeRecognizedAs(tree);
		}

		private LanguageConstruct BlankLineIndented(int indentation)
		{
			return new BlankLine(PossiblySpecified<int>.WithValue(indentation), ParseError.NoErrors);
		}

		private static CommentDefinition Comment(int commentId, string content)
		{
			return new CommentDefinition(PossiblySpecified<bool>.Unspecifed, commentId, content, ParseError.NoErrors);
		}

		private static UnknownStatement StatementIndented(int indentationLevel, string content)
		{
			return new UnknownStatement(PossiblySpecified<bool>.Unspecifed, PossiblySpecified<int>.WithValue(indentationLevel), content, Enumerable.Empty<int>(), ParseError.NoErrors);
		}

		private static UnknownPrelude PreludeIndented(int indentationLevel, string content)
		{
			return new UnknownPrelude(PossiblySpecified<int>.WithValue(indentationLevel), content, Enumerable.Empty<int>(), ParseError.NoErrors);
		}

		[NotNull, SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")] private City _city;
		[NotNull, SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")] private District _arbitraryDistrict;
		private const string ArbitraryComment = "comment def";
		private const string ArbitraryContent = "arbitrary.content";

		[SetUp]
		public void Setup()
		{
			_city = new City();
			_arbitraryDistrict = _city.District("CompileTest");
		}
	}
}
