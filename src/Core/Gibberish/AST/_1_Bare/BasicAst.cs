using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Gibberish.AST._1_Bare
{
	public static class BasicAst
	{
		public static StatementBuilder Statement([NotNull] string content)
		{
			return new StatementBuilder(content);
		}

		public static BlockBuilder Block(string prelude)
		{
			return new BlockBuilder(prelude);
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

			public Builder WithError(ParseError error)
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
			public StatementBuilder(string content)
			{
				Content = content;
			}

			[NotNull]
			public string Content { get; set; }

			internal override void Build(List<LanguageConstruct> destination)
			{
				destination.Add(new UnknownStatement(Content, Errors));
			}
		}

		public class BlockBuilder : Builder
		{
			public BlockBuilder(string prelude)
			{
				Prelude = prelude;
			}

			[NotNull]
			public string Prelude { get; set; }

			[NotNull]
			public BlockBuilder WithBody([NotNull] Action<BodyBuilder> options)
			{
				options(new BodyBuilder(this));
				return this;
			}

			public class BodyBuilder
			{
				public BodyBuilder(BlockBuilder self)
				{
					_self = self;
				}

				public StatementBuilder AddStatement(string content)
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
				var body = new List<LanguageConstruct>();
				foreach (var statement in Body) { statement.Build(body); }
				destination.Add(new UnknownBlock(Prelude, body));
			}
		}
	}
}
