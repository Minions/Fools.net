using System;
using System.Collections.Generic;
using System.Linq;
using Gibberish.Parsing;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Gibberish.AST._1_Bare
{
	public static class BasicAst
	{
		[NotNull]
		public static StatementBuilder Statement([NotNull] string content)
		{
			return new StatementBuilder(content);
		}

		[NotNull]
		public static BlockBuilder Block([NotNull] string prelude)
		{
			return Block(prelude, x => { });
		}

		[NotNull]
		public static BlockBuilder Block([NotNull] string prelude, [NotNull] Action<BlockBuilder.PreludeBuilder> func)
		{
			return new BlockBuilder(prelude, func);
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

			public override string ToString()
			{
				return JsonConvert.SerializeObject(this, NoWhitespace);
			}

			[NotNull]
			public List<ParseError> Errors { get; set; } = Recognition.NoErrors.ToList();

			[NotNull]
			public Builder WithError([NotNull] ParseError error)
			{
				Errors.Add(error);
				return this;
			}

			internal abstract void Build([NotNull] List<LanguageConstruct> destination);

			private static readonly JsonSerializerSettings NoWhitespace = new JsonSerializerSettings
			{
				Formatting = Formatting.None
			};
		}

		public class StatementBuilder : Builder
		{
			public StatementBuilder([NotNull] string content)
			{
				Content = content;
			}

			[NotNull]
			public string Content { get; set; }

			[NotNull]
			public List<int> Comments { get; } = new List<int>();

			[NotNull]
			public StatementBuilder WithCommentRefs([NotNull] params int[] indices)
			{
				Comments.AddRange(indices);
				return this;
			}

			internal override void Build(List<LanguageConstruct> destination)
			{
				destination.Add(new UnknownStatement(Content, Comments, Errors));
			}
		}

		public class BlockBuilder : Builder
		{
			public BlockBuilder([NotNull] string prelude, [NotNull] Action<PreludeBuilder> func)
			{
				Prelude = new PreludeBuilder(prelude);
				func(Prelude);
			}

			[NotNull]
			public PreludeBuilder Prelude { get; }

			[NotNull]
			public BlockBuilder WithBody([NotNull] Action<BodyBuilder> options)
			{
				options(new BodyBuilder(this));
				return this;
			}

			public class PreludeBuilder : Builder
			{
				public PreludeBuilder([NotNull] string content)
				{
					Content = content;
				}

				[NotNull]
				public string Content { get; set; }

				[NotNull]
				public List<int> Comments { get; } = new List<int>();

				[NotNull]
				public PreludeBuilder WithCommentRefs([NotNull] params int[] indices)
				{
					Comments.AddRange(indices);
					return this;
				}

				internal override void Build(List<LanguageConstruct> destination)
				{
					destination.Add(new UnknownPrelude(Content, Comments, Errors));
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
					var statement = Statement(content);
					_self.Body.Add(statement);
					return statement;
				}

				[NotNull] private readonly BlockBuilder _self;
			}

			[NotNull]
			public List<StatementBuilder> Body { get; } = new List<StatementBuilder>();

			internal override void Build(List<LanguageConstruct> destination)
			{
				var prelude = new List<LanguageConstruct>();
				Prelude.Build(prelude);
				var body = new List<LanguageConstruct>();
				foreach (var statement in Body) { statement.Build(body); }
				destination.Add(new UnknownBlock((UnknownPrelude) prelude[0], body, Errors));
			}
		}
	}
}
