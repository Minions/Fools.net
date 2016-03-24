using System.Collections.Generic;
using System.Linq;
using Gibberish.Parsing;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	public class Parse
	{
		private Parse([NotNull] IEnumerable<Declaration> declarations, IEnumerable<Statement> statements, [CanBeNull] NameNode maybeName, IEnumerable<string> texts,
			[NotNull] IEnumerable<ParseError> parseErrors)
		{
			Declarations = declarations;
			Statements = statements;
			MaybeName = maybeName;
			Texts = texts;
			Errors = parseErrors;
		}

		[NotNull] public static readonly IEnumerable<ParseError> NoErrors = Enumerable.Empty<ParseError>();
		[NotNull] private static readonly IEnumerable<Declaration> NoDeclarations = Enumerable.Empty<Declaration>();
		[NotNull] private static readonly IEnumerable<Statement> NoStatements = Enumerable.Empty<Statement>();
		[NotNull] private static readonly IEnumerable<string> NoTexts = Enumerable.Empty<string>();
		[NotNull] public static readonly Parse Empty = WithErrors();

		[NotNull] public readonly IEnumerable<Declaration> Declarations;
		[NotNull] public readonly IEnumerable<Statement> Statements;
		[CanBeNull] public readonly NameNode MaybeName;
		[NotNull] public readonly IEnumerable<string> Texts;
		[NotNull] public readonly IEnumerable<ParseError> Errors;

		public static Parse WithErrors([NotNull] params ParseError[] parseErrors)
		{
			return new Parse(NoDeclarations, NoStatements, null, NoTexts, parseErrors);
		}

		public static Parse Valid([NotNull] Declaration declaration, [NotNull] IEnumerable<ParseError> parseErrors)
		{
			return new Parse(
				new[]
				{
					declaration
				},
				NoStatements,
				null,
				NoTexts,
				parseErrors);
		}

		public static Parse Valid(Statement statement, IEnumerable<ParseError> parseErrors)
		{
			return new Parse(
				NoDeclarations,
				new[]
				{
					statement
				},
				null,
				NoTexts,
				parseErrors);
		}

		[NotNull]
		public static Parse Valid([NotNull] NameNode name, [NotNull] IEnumerable<ParseError> parseErrors)
		{
			return new Parse(NoDeclarations, NoStatements, name, NoTexts, parseErrors);
		}

		public static Parse Text(string data)
		{
			return new Parse(
				NoDeclarations,
				NoStatements,
				null,
				new[]
				{
					data
				},
				NoErrors);
		}

		[NotNull]
		public static ParseError[] AllErrors([NotNull] params Parse[] parses)
		{
			return parses.SelectMany(p => p.Errors)
				.ToArray();
		}

		[CanBeNull]
		public static Parse MergeAll([NotNull] params Parse[] parses)
		{
			return MergeAll((IEnumerable<Parse>) parses);
		}

		[NotNull]
		public static Parse MergeAll([NotNull] IEnumerable<Parse> parses)
		{
			var enumOnce = parses as Parse[] ?? parses.ToArray();
			return new Parse(
				enumOnce.SelectMany(p => p.Declarations),
				enumOnce.SelectMany(p => p.Statements),
				enumOnce.Aggregate((NameNode) null, (current, next) => current == null ? next.MaybeName : current),
				enumOnce.SelectMany(p => p.Texts),
				enumOnce.SelectMany(p => p.Errors));
		}
	}
}
