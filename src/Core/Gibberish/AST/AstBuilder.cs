using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Gibberish.AST
{
	public abstract class AstBuilder<TConstruct>
	{
		[NotNull]
		public List<TConstruct> Build()
		{
			var statements = new List<TConstruct>();
			Build(statements);
			return statements;
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this, NoWhitespace);
		}

		[NotNull]
		public List<ParseError> Errors { get; } = ParseError.NoErrors.ToList();

		[NotNull]
		public AstBuilder<TConstruct> WithError([NotNull] ParseError error)
		{
			Errors.Add(error);
			return this;
		}

		internal abstract void Build([NotNull] List<TConstruct> destination);

		private static readonly JsonSerializerSettings NoWhitespace = new JsonSerializerSettings
		{
			Formatting = Formatting.None
		};
	}
}
