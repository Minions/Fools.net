using System;
using System.Collections.Generic;
using System.Linq;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using IronMeta.Matcher;
using JetBrains.Annotations;

namespace Gibberish.Parsing
{
	internal static class ParsingExtensions
	{
		[NotNull]
		public static Parse AsParse([NotNull] this ParseError parseErrors)
		{
			return Parse.WithErrors(parseErrors);
		}

		[NotNull]
		public static Parse AsParse([NotNull] this IEnumerable<ParseError> parseErrors)
		{
			return Parse.WithErrors(parseErrors.ToArray());
		}

		[NotNull]
		public static Recognition AsRecognition([NotNull] this ParseError parseErrors)
		{
			return Recognition.WithUnattachedErrors(new []{parseErrors});
		}

		[NotNull]
		public static Recognition AsRecognition([NotNull] this IEnumerable<ParseError> parseErrors)
		{
			return Recognition.WithUnattachedErrors(parseErrors);
		}

		[NotNull]
		public static Recognition AsRecognition([NotNull] this LanguageConstruct item)
		{
			return Recognition.With(item);
		}

		[NotNull]
		public static string ToSetDisplayString([NotNull] this IEnumerable<string> values)
		{
			return "{" + String.Join(", ", values) + "}";
		}

		[NotNull]
		public static Block ExtractBlock([NotNull] this MatchItem<char, Parse> matchItem)
		{
			return (Block) matchItem.Results.Single()
				.Statements.Single();
		}

		[NotNull]
		public static Parse WithErrorsFrom([NotNull] this Parse main, [NotNull] params MatchItem<char, Parse>[] errorSources)
		{
			return Parse.MergeAll(
				main,
				Parse.WithErrors(
					errorSources.SelectMany(p => p.ParseErrors())
						.ToArray()));
		}

		[NotNull]
		public static string AsString<T>([NotNull] this MatchItem<char, T> sp)
		{
			return new string(sp.Inputs.ToArray());
		}

		[NotNull]
		public static IEnumerable<ParseError> ParseErrors([NotNull] this MatchItem<char, Parse> errs)
		{
			return errs.Results.SelectMany(p => p.Errors);
		}
	}
}
