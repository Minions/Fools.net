using Gibberish.AST;
using IronMeta.Matcher;
using Newtonsoft.Json;

namespace MatchResultExtensions
{
	internal static class MatchResultExtensions
	{
		public static string PrettyPrint(this MatchResult<char, ParseTree> self)
		{
			if (self.Success) { return "Success:\r\n" + (self.Result == null ? "<null>" : JsonConvert.SerializeObject(self.Result)); }
			return "Error: " + (self.Error ?? "<null>");
		}
	}
}