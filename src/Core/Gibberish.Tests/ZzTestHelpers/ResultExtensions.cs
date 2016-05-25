using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using Gibberish.AST._1_Bare;
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

		[NotNull]
		public static RecognitionAssertions<LanguageConstruct> Should([NotNull] this IEnumerable<LanguageConstruct> result)
		{
			return new RecognitionAssertions<LanguageConstruct>(true, result);
		}

		private static readonly JsonSerializerSettings WithTypeNames = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Objects,
			TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
		};
	}
}
