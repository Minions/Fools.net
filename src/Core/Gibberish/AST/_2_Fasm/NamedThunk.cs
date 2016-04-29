using System.Collections.Generic;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Gibberish.AST._2_Fasm
{
	internal class NamedThunk : FasmConstruct
	{
		public NamedThunk([NotNull] string name, [NotNull] List<FasmOrLanguageConstruct> body)
		{
			Name = name;
			Body = body;
		}

		[NotNull]
		public string Name { get; }
		[NotNull]
		public List<FasmOrLanguageConstruct> Body { get; }
	}
}
