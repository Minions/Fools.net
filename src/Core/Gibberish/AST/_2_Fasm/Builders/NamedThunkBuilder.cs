using System;
using System.Collections.Generic;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Gibberish.AST._2_Fasm.Builders
{
	public class NamedThunkBuilder : AstBuilder<FasmOrLanguageConstruct>
	{
		public NamedThunkBuilder([NotNull] string name)
		{
			Name = name;
		}

		[NotNull]
		public string Name { get; }

		public NamedThunkBuilder WithBody(Action<BodyBuilder> defineBody)
		{
			defineBody(new BodyBuilder(this));
			return this;
		}

		public class BodyBuilder
		{
			public BodyBuilder(NamedThunkBuilder self)
			{
				_self = self;
			}

			public BodyBuilder AddUnknownStatement(string unparsedContent, int indentationDepth)
			{
				_self.Body.Add(new UnknownStatementBuilder(unparsedContent, indentationDepth));
				return this;
			}

			private readonly NamedThunkBuilder _self;
		}

		[NotNull]
		public List<AstBuilder<FasmOrLanguageConstruct>> Body { get; } = new List<AstBuilder<FasmOrLanguageConstruct>>();

		public override void BuildInto(List<FasmOrLanguageConstruct> destination)
		{
			var body = new List<FasmOrLanguageConstruct>();
			foreach (var builder in Body) { builder.BuildInto(body); }
			destination.Add(new NamedThunk(Name, body));
		}
	}
}
