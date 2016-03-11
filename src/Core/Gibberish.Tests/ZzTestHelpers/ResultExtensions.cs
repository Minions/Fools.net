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
		public static string PrettyPrint([CanBeNull] this MatchResult<char, ParseTree> self)
		{
			if (self == null) { return "No match result"; }
			if (self.Success) { return "Success:\r\n" + (self.Result == null ? "<null>" : JsonConvert.SerializeObject(self.Result)); }
			return "Error: " + (self.Error ?? "<null>");
		}

		public static void ShouldHave([CanBeNull] this ThunkDescriptor subject, object expectation) { subject.ShouldBeEquivalentTo(expectation, c => c.ExcludingMissingMembers()); }
	}
}
