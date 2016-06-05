using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
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
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				BasicAst.SequenceOfRawLines(
					f => f.Block("outer block")
						.WithBody(
							b =>
							{
								b.AddStatement("outer 1");
								b.AddStatement("outer 2");
							}))
					.Build());
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f => f.Block("outer block")
							.WithBody(
								b =>
								{
									b.AddStatement("outer 1");
									b.AddStatement("outer 2");
								})));
		}

		[Test]
		public void BlocksShouldBeNestable()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				BasicAst.SequenceOfRawLines(
					f => f.Block("outer block")
						.WithBody(
							b =>
							{
								b.AddBlock("nested 1")
									.WithBody(inner => inner.AddStatement("nested 1.1"));
							}))
					.Build());
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f => f.Block("outer block")
							.WithBody(
								b =>
								{
									b.AddBlock("nested 1")
										.WithBody(inner => inner.AddStatement("nested 1.1"));
								})));
		}

		[Test]
		public void FollowingABlockWithALessNestedStatementEndsTheInnerBlock()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				BasicAst.SequenceOfRawLines(
					f => f.Block("outer block")
						.WithBody(
							b =>
							{
								b.AddBlock("nested 1")
									.WithBody(inner => inner.AddStatement("nested 1.1"));
								b.AddStatement("outer 1");
							}))
					.Build());
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f => f.Block("outer block")
							.WithBody(
								b =>
								{
									b.AddBlock("nested 1")
										.WithBody(inner => inner.AddStatement("nested 1.1"));
									b.AddStatement("outer 1");
								})));
		}

		[Test]
		public void FollowingABlockWithALessNestedBlockEndsTheInnerBlockAndStartsANewOne()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				BasicAst.SequenceOfRawLines(
					f => f.Block("outer block")
						.WithBody(
							b =>
							{
								b.AddBlock("nested 1")
									.WithBody(inner => inner.AddStatement("nested 1.1"));
								b.AddBlock("nested 2")
									.WithBody(inner => inner.AddStatement("nested 2.1"));
							}))
					.Build());
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f => f.Block("outer block")
							.WithBody(
								b =>
								{
									b.AddBlock("nested 1")
										.WithBody(inner => inner.AddStatement("nested 1.1"));
									b.AddBlock("nested 2")
										.WithBody(inner => inner.AddStatement("nested 2.1"));
								})));
		}

		[Test]
		public void IndentingAStatementBeyondCurrentLevelGivesAnError()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				new List<LanguageConstruct>
				{
					StatementIndented(0, ArbitraryContent),
					StatementIndented(1, ArbitraryContent)
				});
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f =>
						{
							f.Statement(ArbitraryContent);
							f.Statement(ArbitraryContent)
								.WithError(ParseError.IncorrectIndentation(0, 1));
						}));
		}

		[Test]
		public void IndentingAPreludeBeyondCurrentLevelGivesAnError()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				new List<LanguageConstruct>
				{
					StatementIndented(0, ArbitraryContent),
					PreludeIndented(1, ArbitraryContent),
					StatementIndented(2, ArbitraryContent)
				});
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f =>
						{
							f.Statement(ArbitraryContent);
							f.Block(ArbitraryContent)
								.WithBody(b => b.AddStatement(ArbitraryContent))
								.WithError(ParseError.WholeBlockIsIndentedTooFar(0, 1));
						}));
		}

		[Test]
		public void IndentingJustThePreludeLineTooFarGivesSpecialError()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				new List<LanguageConstruct>
				{
					StatementIndented(0, ArbitraryContent),
					PreludeIndented(3, ArbitraryContent),
					StatementIndented(1, ArbitraryContent)
				});
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f =>
						{
							f.Statement(ArbitraryContent);
							f.Block(ArbitraryContent)
								.WithBody(b => b.AddStatement(ArbitraryContent))
								.WithError(ParseError.IncorrectIndentation(0, 3));
						}));
		}

		[Test]
		public void IndentingThePreludeLineTooFarWithNoBlockBodyGivesCorrectErrors()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				new List<LanguageConstruct>
				{
					StatementIndented(0, ArbitraryContent),
					PreludeIndented(3, ArbitraryContent),
					StatementIndented(0, ArbitraryContent)
				});
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f =>
						{
							f.Statement(ArbitraryContent);
							f.Block(ArbitraryContent)
								.WithError(ParseError.IncorrectIndentation(0, 3))
								.WithError(ParseError.MissingBody());
							f.Statement(ArbitraryContent);
						}));
		}

		[Test]
		public void BadPreludeIndentFollowedByMoreBadIndentingThatCannotBeInterpretedAnyOtherWayResultsInIndependentErrors()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				new List<LanguageConstruct>
				{
					StatementIndented(0, ArbitraryContent),
					PreludeIndented(3, ArbitraryContent),
					StatementIndented(2, ArbitraryContent)
				});
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f =>
						{
							f.Statement(ArbitraryContent);
							f.Block(ArbitraryContent)
								.WithError(ParseError.IncorrectIndentation(0, 3))
								.WithError(ParseError.MissingBody());
							f.Statement(ArbitraryContent)
								.WithError(ParseError.IncorrectIndentation(0, 2));
						}));
		}

		[Test]
		public void BadPreludeIndentFollowedByMoreBadIndentingInAnotherWayThatCannotBeInterpretedAnyOtherWayResultsInIndependentErrors()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				new List<LanguageConstruct>
				{
					StatementIndented(0, ArbitraryContent),
					PreludeIndented(3, ArbitraryContent),
					PreludeIndented(5, ArbitraryContent)
				});
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f =>
						{
							f.Statement(ArbitraryContent);
							f.Block(ArbitraryContent)
								.WithError(ParseError.IncorrectIndentation(0, 3))
								.WithError(ParseError.MissingBody());
							f.Block(ArbitraryContent)
								.WithError(ParseError.IncorrectIndentation(0, 5))
								.WithError(ParseError.MissingBody());
						}));
		}

		[Test]
		public void PreludeFollowedByNonIndentedStatementGivesError()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				new List<LanguageConstruct>
				{
					PreludeIndented(0, ArbitraryContent),
					StatementIndented(0, ArbitraryContent)
				});
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f =>
						{
							f.Block(ArbitraryContent)
								.WithError(ParseError.MissingBody());
							f.Statement(ArbitraryContent);
						}));
		}

		[Test]
		public void PreludeFollowedCommentDefinitionGivesError()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				new List<LanguageConstruct>
				{
					PreludeIndented(0, ArbitraryContent),
					Comment(1, ArbitraryContent)
				});
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f =>
						{
							f.Block(ArbitraryContent)
								.WithError(ParseError.MissingBody());
							f.CommentDefinition(1, ArbitraryContent)
								.ThatStartsParagraph();
						}));
		}

		[Test]
		public void BlankLinesShouldStartNewParagraphAndMaintainSameBlock()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				BasicAst.SequenceOfRawLines(
					f => f.Block("outer block")
						.WithBody(
							b =>
							{
								b.AddStatement("outer 1");
								b.AddBlankLine();
								b.AddStatement("outer 2");
							}))
					.Build());
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f => f.Block("outer block")
							.WithBody(
								b =>
								{
									b.AddStatement("outer 1");
									b.AddStatement("outer 2")
										.ThatStartsNewParagraph();
								})));
		}

		[Test]
		public void MultipleBlankLinesShouldBeTreatedLikeOne()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				BasicAst.SequenceOfRawLines(
					f =>
					{
						f.Statement("1");
						f.BlankLine();
						f.BlankLine();
						f.Statement("2");
					})
					.Build());
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f =>
						{
							f.Statement("1");
							f.Statement("2")
								.ThatStartsNewParagraph();
						}));
		}

		[Test]
		public void BlankLinesBeforeBlocksShouldStartNewParagraph()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				BasicAst.SequenceOfRawLines(
					f =>
					{
						f.Statement("1");
						f.BlankLine();
						f.Block("outer")
							.WithBody(b => b.AddStatement("outer 1"));
					})
					.Build());
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f =>
						{
							f.Statement("1");
							f.Block("outer")
								.WithBody(b => b.AddStatement("outer 1"))
								.ThatStartsNewParagraph();
						}));
		}

		[Test]
		public void CommentDefinitionsShouldTerminatePreceedingBlocksAndAppearAtRootLevelAsNewParagraph()
		{
			var testSubject = new AssembleBlocks();
			var result = testSubject.Transform(
				BasicAst.SequenceOfRawLines(
					f =>
					{
						f.Block("outer block")
							.WithBody(b => { b.AddStatement("outer 1"); });
						f.CommentDefinition(3, ArbitraryComment);
					})
					.Build());
			result.Should()
				.BeRecognizedAs(
					BasicAst.BlockTree(
						f =>
						{
							f.Block("outer block")
								.WithBody(b => { b.AddStatement("outer 1"); });
							f.CommentDefinition(3, ArbitraryComment)
								.ThatStartsParagraph();
						}));
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
						f.BlankLine(0);
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
