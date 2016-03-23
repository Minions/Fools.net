using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Gibberish.AST._1_Bare
{
	public static class BasicAst
	{
		[NotNull]
		public static StatementBuilder Statement([NotNull] string content)
		{
			return new StatementBuilder(content, 0);
		}

		[NotNull]
		public static BlockBuilder Block([NotNull] string prelude)
		{
			return Block(prelude, x => { });
		}

		[NotNull]
		public static BlockBuilder Block([NotNull] string prelude, [NotNull] Action<BlockBuilder.PreludeBuilder> preludeOptions)
		{
			return new BlockBuilder(prelude, preludeOptions, 0);
		}

		public static CommentBuilder CommentDefinition(int commentId, string content)
		{
			return new CommentBuilder(commentId, content);
		}

		public abstract class Builder
		{
			[NotNull]
			public List<LanguageConstruct> Build()
			{
				var statements = new List<LanguageConstruct>();
				Build(statements);
				return statements;
			}

			[NotNull]
			public List<LanguageConstruct> BuildRaw()
			{
				var statements = new List<LanguageConstruct>();
				BuildRaw(statements);
				return statements;
			}

			public override string ToString()
			{
				return JsonConvert.SerializeObject(this, NoWhitespace);
			}

			[NotNull]
			public List<ParseError> Errors { get; } = ParseError.NoErrors.ToList();

			[NotNull]
			public Builder WithError([NotNull] ParseError error)
			{
				Errors.Add(error);
				return this;
			}

			internal abstract void Build([NotNull] List<LanguageConstruct> destination);

			internal abstract void BuildRaw([NotNull] List<LanguageConstruct> destination);

			private static readonly JsonSerializerSettings NoWhitespace = new JsonSerializerSettings
			{
				Formatting = Formatting.None
			};
		}

		public class StatementBuilder : Builder
		{
			public StatementBuilder([NotNull] string content, int indentationDepth)
			{
				Content = content;
				IndentationDepth = indentationDepth;
			}

			[NotNull]
			public string Content { get; }

			[NotNull]
			public List<int> Comments { get; } = new List<int>();

			public int IndentationDepth { get; }

			[NotNull]
			public StatementBuilder WithCommentRefs([NotNull] params int[] indices)
			{
				Comments.AddRange(indices);
				return this;
			}

			internal override void Build(List<LanguageConstruct> destination)
			{
				destination.Add(new UnknownStatement(IndentationDepth, Content, Comments, Errors));
			}

			internal override void BuildRaw(List<LanguageConstruct> destination)
			{
				Build(destination);
			}
		}

		public class BlockBuilder : Builder
		{
			public BlockBuilder([NotNull] string prelude, [NotNull] Action<PreludeBuilder> func, int indentationDepth)
			{
				IndentationDepth = indentationDepth;
				Prelude = new PreludeBuilder(prelude, indentationDepth);
				func(Prelude);
			}

			public int IndentationDepth { get; }

			[NotNull]
			public PreludeBuilder Prelude { get; }

			[NotNull]
			public BlockBuilder WithBody([NotNull] Action<BodyBuilder> bodyOptions)
			{
				bodyOptions(new BodyBuilder(this));
				return this;
			}

			public class PreludeBuilder : Builder
			{
				public PreludeBuilder([NotNull] string content, int indentationDepth)
				{
					Content = content;
					IndentationDepth = indentationDepth;
				}

				[NotNull]
				public string Content { get; }

				[NotNull]
				public List<int> Comments { get; } = new List<int>();

				[NotNull]
				public PreludeBuilder WithCommentRefs([NotNull] params int[] indices)
				{
					Comments.AddRange(indices);
					return this;
				}

				public int IndentationDepth { get; }

				internal override void Build(List<LanguageConstruct> destination)
				{
					destination.Add(new UnknownPrelude(IndentationDepth, Content, Comments, Errors));
				}

				internal override void BuildRaw(List<LanguageConstruct> destination)
				{
					Build(destination);
				}
			}

			public class BodyBuilder
			{
				public BodyBuilder([NotNull] BlockBuilder self)
				{
					_self = self;
				}

				[NotNull]
				public StatementBuilder AddStatement([NotNull] string content)
				{
					var statement = new StatementBuilder(content, _self.IndentationDepth + 1);
					_self.Body.Add(statement);
					return statement;
				}

				[NotNull]
				public BlockBuilder AddBlock([NotNull] string prelude)
				{
					return AddBlock(prelude, _ => { });
				}

				[NotNull]
				private BlockBuilder AddBlock([NotNull] string prelude, [NotNull] Action<PreludeBuilder> preludeOptions)
				{
					var result = new BlockBuilder(prelude, preludeOptions, _self.IndentationDepth + 1);
					_self.Body.Add(result);
					return result;
				}

				[NotNull] private readonly BlockBuilder _self;
			}

			[NotNull]
			public List<Builder> Body { get; } = new List<Builder>();

			internal override void Build(List<LanguageConstruct> destination)
			{
				var prelude = new List<LanguageConstruct>();
				var body = new List<LanguageConstruct>();
				Prelude.Build(prelude);
				foreach (var builder in Body) { builder.Build(body); }
				destination.Add(new UnknownBlock((UnknownPrelude) prelude[0], body, Errors));
			}

			internal override void BuildRaw(List<LanguageConstruct> destination)
			{
				Prelude.BuildRaw(destination);
				foreach (var builder in Body) { builder.BuildRaw(destination); }
			}
		}

		public class CommentBuilder : Builder
		{
			public CommentBuilder(int commentId, [NotNull] string content)
			{
				CommentId = commentId;
				Content = content;
			}

			public int CommentId { get; }
			[NotNull]
			public string Content { get; }

			internal override void Build(List<LanguageConstruct> destination)
			{
				destination.Add(new CommentDefinition(CommentId, Content, Errors));
			}

			internal override void BuildRaw(List<LanguageConstruct> destination)
			{
				Build(destination);
			}
		}
	}
}
