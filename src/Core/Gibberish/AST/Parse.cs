using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.AST
{
	public class Parse
	{
		private Parse([NotNull] IEnumerable<Declaration> declarations, IEnumerable<Statement> statements, [CanBeNull] NameNode maybeName, [NotNull] IEnumerable<ParseError> parseErrors)
		{
			Declarations = declarations;
			Statements = statements;
			MaybeName = maybeName;
			Errors = parseErrors;
		}

		[NotNull] public static readonly Parse Empty = WithErrors();
		[NotNull] public static readonly IEnumerable<ParseError> NoErrors = Enumerable.Empty<ParseError>();

		[NotNull] public readonly IEnumerable<Declaration> Declarations;
		[NotNull] public readonly IEnumerable<Statement> Statements;
		[CanBeNull] public readonly NameNode MaybeName;
		[NotNull] public readonly IEnumerable<ParseError> Errors;

		public static Parse WithErrors([NotNull] params ParseError[] parseErrors)
		{
			return new Parse(NoDeclarations, NoStatements, null, parseErrors);
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
				parseErrors);
		}

		[NotNull]
		public static Parse MergeAll([NotNull] IEnumerable<Parse> parses)
		{
			var enumOnce = parses as IList<Parse> ?? parses.ToList();
			return new Parse(enumOnce.SelectMany(p => p.Declarations), enumOnce.SelectMany(p => p.Statements), null, enumOnce.SelectMany(p => p.Errors));
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
				parseErrors);
		}

		[NotNull]
		public static Parse Valid([NotNull] NameNode name, [NotNull] IEnumerable<ParseError> parseErrors)
		{
			return new Parse(NoDeclarations, NoStatements, name, parseErrors);
		}

		[NotNull] private static readonly IEnumerable<Declaration> NoDeclarations = Enumerable.Empty<Declaration>();
		[NotNull] private static readonly IEnumerable<Statement> NoStatements = Enumerable.Empty<Statement>();
	}
}
