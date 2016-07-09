using System;
using JetBrains.Annotations;

namespace Gibberish.Parsing.TransformPipeline
{
	public class FunctionCaller<TOut> : PipeHead<TOut>
	{
		public FunctionCaller([NotNull] Action<TOut> listener)
		{
			_listener = listener;
		}

		public void Consume(TOut val)
		{
			_listener(val);
		}

		[NotNull] private readonly Action<TOut> _listener;
	}
}
