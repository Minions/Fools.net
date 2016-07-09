using System.Collections.Generic;
using System.Linq;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Lair
{
	internal class ErrorFinder : LanguageConstructStatefulVisitor
	{
		[NotNull]
		public List<ErrorDescription> Errors { get; } = new List<ErrorDescription>();
		public int _currentLineNumber = 1;

		public void Visit(BlankLine line)
		{
			Errors.AddRange(line.Errors.Select(e => new ErrorDescription(_currentLineNumber, "<empty line>", e, line)));
			++_currentLineNumber;
		}

		public void Visit(UnknownStatement statement)
		{
			Errors.AddRange(statement.Errors.Select(e => new ErrorDescription(_currentLineNumber, $"{FirstPart(statement.Content)}", e, statement)));
			++_currentLineNumber;
		}

		public void Visit(CommentDefinition commentDefinition)
		{
			Errors.AddRange(
				commentDefinition.Errors.Select(
					e => new ErrorDescription(_currentLineNumber, $"#[{commentDefinition.CommentId}]: {FirstPart(commentDefinition.Content)}", e, commentDefinition)));
			++_currentLineNumber;
		}

		public void Visit(UnknownPrelude prelude)
		{
			Errors.AddRange(prelude.Errors.Select(e => new ErrorDescription(_currentLineNumber, $"{FirstPart(prelude.Content)}", e, prelude)));
			++_currentLineNumber;
		}

		public void Visit(UnknownBlock block)
		{
			Errors.AddRange(block.Errors.Select(e => new ErrorDescription(_currentLineNumber, $"<block starting with {FirstPart(block.Prelude.Content)}>", e, block)));
			block.Prelude.Accept(this);
			foreach (var statement in block.Body) { statement.Accept(this); }
		}

		public void Visit(CommentDefinitionBlockPrelude prelude)
		{
			Errors.AddRange(prelude.Errors.Select(e => new ErrorDescription(_currentLineNumber, $"##[{prelude.CommentId}]:", e, prelude)));
			++_currentLineNumber;
		}

		public void Visit(CommentDefinitionBlockStatement statement)
		{
			Errors.AddRange(statement.Errors.Select(e => new ErrorDescription(_currentLineNumber, $"{FirstPart(statement.Content)}", e, statement)));
			++_currentLineNumber;
		}

		public void Visit(CommentDefinitionBlock block)
		{
			Errors.AddRange(block.Errors.Select(e => new ErrorDescription(_currentLineNumber, $"<comment definition block for comment {block.Prelude.CommentId}>", e, block)));
			block.Prelude.Accept(this);
			foreach (var statement in block.Body) { statement.Accept(this); }
		}

		private static string FirstPart(string content)
		{
			if (content.Length < 15) return content;
			return content.Substring(0, 15) + "...";
		}
	}
}
