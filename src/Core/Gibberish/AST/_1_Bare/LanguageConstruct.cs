using System.Collections.Generic;
using System.Linq;
using Gibberish.Parsing;
using JetBrains.Annotations;

namespace Gibberish.AST._1_Bare
{
	public class LanguageConstruct : FasmOrLanguageConstruct
	{
		protected LanguageConstruct([NotNull] IEnumerable<ParseError> errors)
		{
			Errors = errors.ToList();
		}

		[NotNull]
		public List<ParseError> Errors { get; }

		public bool Accept(LanguageConstructVisitor worker, int level, List<LanguageConstruct> result)
		{
			if (GetType() == typeof (UnknownStatement)) { return worker.Visit((UnknownStatement) this, level, result); }
			if (GetType() == typeof (BlankLine)) { return worker.Visit((BlankLine) this, level, result); }
			if (GetType() == typeof (CommentDefinition)) { return worker.Visit((CommentDefinition) this, level, result); }
			if (GetType() == typeof (UnknownPrelude)) { return worker.Visit((UnknownPrelude) this, level, result); }
			if (GetType() == typeof (UnknownBlock)) { return worker.Visit((UnknownBlock) this, level, result); }
			throw new UnfixableError($"a new type got added to the concrete children of {typeof (LanguageConstruct).Name}, but the Accept function does not know the type. Found value of type {GetType().Name}, value {this}.");
		}
	}
}
