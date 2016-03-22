using System.Runtime.Serialization.Formatters;
using FluentAssertions;
using FluentAssertions.Primitives;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using Gibberish.Execution;
using Gibberish.Tests.RecognizeBlockSyntax;
using IronMeta.Matcher;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Gibberish.Tests.ZzTestHelpers
{
	internal static class ResultExtensions
	{
		[NotNull]
		public static string PrettyPrint<T>([CanBeNull] this MatchResult<char, T> self)
		{
			if (self == null) { return "No match result"; }
			if (!self.Success) { return "Unhandled low-level error: " + (self.Error ?? "<null>"); }
			if (self.Result == null) { return "Low-level parser matched, but result was <null>"; }
			return JsonConvert.SerializeObject(self.Result, WithTypeNames);
		}

		public static void ShouldHave([CanBeNull] this ThunkDescriptor subject, object expectation)
		{
			subject.ShouldBeEquivalentTo(expectation, c => c.ExcludingMissingMembers());
		}

		[NotNull]
		public static ParseResultAssertions Declare([NotNull] this ParseResultAssertions target, [NotNull] Declaration declaration)
		{
			return ParseAs(target, Parse.Valid(declaration, Parse.NoErrors));
		}

		[NotNull]
		public static ParseResultAssertions Should([NotNull] this MatchResult<char, Parse> parseResult)
		{
			return new ParseResultAssertions(parseResult);
		}

		[NotNull]
		public static RecognitionAssertions Should([NotNull] this MatchResult<char, Recognition> result)
		{
			var recognition = result.Success ? result.Result : null;
			return new RecognitionAssertions(result.Success, recognition);
		}

		[NotNull]
		public static ParseResultAssertions ParseAs([NotNull] this ParseResultAssertions target, [NotNull] Parse expectation)
		{
			target.Subject.ShouldBeEquivalentTo(
				new
				{
					Success = true
				},
				options => options.ExcludingMissingMembers());
			target.Parse.ShouldBeEquivalentTo(
				expectation,
				options => options.IncludingFields()
					.IncludingNestedObjects()
					.RespectingRuntimeTypes());
			return target;
		}

		[NotNull]
		public static ParseResultAssertions ParseWithErrors([NotNull] this ParseResultAssertions target, [NotNull] ParseError expected)
		{
			target.Subject.ShouldBeEquivalentTo(
				new
				{
					Success = true
				},
				options => options.ExcludingMissingMembers());
			target.Parse.ShouldBeEquivalentTo(
				new
				{
					Errors = new[]
					{
						expected
					}
				},
				options => options.ExcludingMissingMembers()
					.IncludingFields()
					.IncludingNestedObjects()
					.RespectingRuntimeTypes());
			return target;
		}

		private static readonly JsonSerializerSettings WithTypeNames = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Objects,
			TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
		};
	}

	internal class RecognitionAssertions : ObjectAssertions
	{
		public RecognitionAssertions(bool success, [NotNull] Recognition result) : base(result)
		{
			Success = success;
			Result = result;
		}

		public bool Success { get; }
		[NotNull]
		public Recognition Result { get; }

		public void BeRecognizedAs(BasicAst.Builder expected)
		{
			var statements = expected.Build();
			Success.Should()
				.BeTrue("parse should have fully matched the input. This is probably an error in the test");
			Result.Items.ShouldBeEquivalentTo(
				statements,
				options => options.IncludingFields()
					.IncludingProperties()
					.IncludingNestedObjects()
					.RespectingRuntimeTypes()
					.IncludingAllRuntimeProperties());
		}
	}
}
