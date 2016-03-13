using FluentAssertions;
using Gibberish.AST;
using Gibberish.Execution;
using IronMeta.Matcher;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Gibberish.Tests.ZzTestHelpers
{
	internal static class ResultExtensions
	{
		[NotNull]
		public static string PrettyPrint([CanBeNull] this MatchResult<char, Parse> self)
		{
			if (self == null) { return "No match result"; }
			if (self.Success)
			{
				if (self.Result == null) { return "Low-level parser matched, but result was <null>"; }
				return JsonConvert.SerializeObject(self.Result);
			}
			return "Unhandled low-level error: " + (self.Error ?? "<null>");
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
	}
}
