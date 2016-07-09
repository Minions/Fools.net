using System;
using System.Collections.Generic;
using Gibberish.AST._1_Bare;
using Gibberish.Parsing.Passes;
using Gibberish.Parsing.TransformPipeline;
using JetBrains.Annotations;

namespace Gibberish.Parsing
{
	public static class LanguageTools
	{
		[NotNull]
		public static LanguagePipelineBuilder Pipeline()
		{
			return new LanguagePipelineBuilder();
		}

		public class LanguagePipelineBuilder
		{
			[NotNull]
			public LanguagePipelineBuilder WithResultListener([NotNull] Action<List<LanguageConstruct>> handleResults)
			{
				_handleResults = handleResults;
				return this;
			}

			[NotNull]
			public LanguagePipelineStart Build()
			{
				var source = new PipeSection<string, List<LanguageConstruct>>(new RecognizeLines());
				var understandFile = source.AddTransformSequence(new AssembleBlocks());
				if (_handleResults != null) { understandFile.AddListener(_handleResults); }
				return new LanguagePipelineStart(source);
			}

			[CanBeNull] private Action<List<LanguageConstruct>> _handleResults;
		}
	}

	public class LanguagePipelineStart
	{
		public LanguagePipelineStart([NotNull] PipeHead<string> source)
		{
			_source = source;
		}

		[NotNull] private readonly PipeHead<string> _source;

		public void Analyze([NotNull] string code)
		{
			_source.Consume(code);
		}
	}
}
