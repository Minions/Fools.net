using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using FluentAssertions;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using Gibberish.Execution;
using IronMeta.Matcher;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Gibberish.Tests.ZzTestHelpers
{
	internal static class ResultExtensions
	{
		[NotNull, UsedImplicitly]
		public static string PrettyPrint<T>([CanBeNull] this MatchResult<char, T> self)
		{
			if (!self.Success) { return "Unhandled low-level error: " + (self.Error ?? "<null>"); }
			if (!self.Results.Any()) { return "Low-level parser matched, but no results found."; }
			return JsonConvert.SerializeObject(self.Results, WithTypeNames);
		}

		[NotNull, UsedImplicitly]
		public static string PrettyPrint([CanBeNull] this object self)
		{
			return JsonConvert.SerializeObject(self, WithTypeNames);
		}

		[UsedImplicitly]
		public static void ApproveJson<T>([CanBeNull] this T self)
		{
			ApprovalTests.Approvals.VerifyJson(self.PrettyPrint());
		}

		public static void ShouldHave([CanBeNull] this ThunkDescriptor subject, object expectation)
		{
			subject.ShouldBeEquivalentTo(expectation, c => c.ExcludingMissingMembers());
		}

		[NotNull]
		public static ParseResultAssertions Should([NotNull] this MatchResult<char, Parse> parseResult)
		{
			return new ParseResultAssertions(parseResult);
		}

		[NotNull]
		public static RecognitionAssertions<LanguageConstruct> Should([NotNull] this MatchResult<char, LanguageConstruct> result)
		{
			var recognition = result.Success ? result.Results : Enumerable.Empty<LanguageConstruct>();
			return new RecognitionAssertions<LanguageConstruct>(result.Success, recognition);
		}

		[NotNull]
		public static RecognitionAssertions<LanguageConstruct> Should([NotNull] this IEnumerable<LanguageConstruct> result)
		{
			return new RecognitionAssertions<LanguageConstruct>(true, result);
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
}
