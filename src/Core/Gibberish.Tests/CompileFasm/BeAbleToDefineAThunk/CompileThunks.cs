using System.Diagnostics.CodeAnalysis;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using Gibberish.Execution;
using Gibberish.Parsing;
using Gibberish.Tests.ZzTestHelpers;
using JetBrains.Annotations;
using NUnit.Framework;

namespace Gibberish.Tests.CompileFasm.BeAbleToDefineAThunk
{
	[TestFixture]
	public class CompileThunks
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

		[Test]
		public void CompilingATrivialDefineThunkNodeShouldCreateThunkInNameTable()
		{
			var testSubject = Tools.Fasm_Old.Compiler_OldApi;
			var parse = FasmAstOld.Thunk(ArbitraryName, FasmAstOld.PassRaw);
			testSubject.CompileFragment(parse, _arbitraryDistrict);
			_arbitraryDistrict.Name(ArbitraryName)
				.ShouldHave(
					new
					{
						name = ArbitraryName
					});
		}

		private const string ArbitraryName = "the.name";

		[NotNull, SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")] private City _city;
		[NotNull, SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")] private District _arbitraryDistrict;
		private const string ArbitraryComment = "comment def";

		[SetUp]
		public void Setup()
		{
			_city = new City();
			_arbitraryDistrict = _city.District("CompileTest");
		}
	}
}
