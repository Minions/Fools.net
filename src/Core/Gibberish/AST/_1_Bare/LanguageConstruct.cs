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
			var myType = GetType();
			if (myType == typeof (UnknownStatement)) { return worker.Visit((UnknownStatement) this, level, result); }
			if (myType == typeof (BlankLine)) { return worker.Visit((BlankLine) this, level, result); }
			if (myType == typeof (CommentDefinition)) { return worker.Visit((CommentDefinition) this, level, result); }
			if (myType == typeof (UnknownPrelude)) { return worker.Visit((UnknownPrelude) this, level, result); }
			if (myType == typeof (UnknownBlock)) { return worker.Visit((UnknownBlock) this, level, result); }
			if (myType == typeof (CommentDefinitionBlockPrelude)) { return worker.Visit((CommentDefinitionBlockPrelude) this, level, result); }
			if (myType == typeof (CommentDefinitionBlockStatement)) { return worker.Visit((CommentDefinitionBlockStatement) this, level, result); }
			if (myType == typeof (CommentDefinitionBlock)) { return worker.Visit((CommentDefinitionBlock) this, level, result); }
			throw new UnfixableError($"a new type got added to the concrete children of {typeof (LanguageConstruct).Name}, but the Accept function does not know the type. Found value of type {myType.Name}, value {this}.");
		}
	}
}
