using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gibberish.Parsing.TransformPipeline
{
	public class PipeTail<TDest>
	{
		public void AddListener(Action<TDest> listener)
		{
			_listeners.Add(new FunctionCaller<TDest>(listener));
		}

		protected PipeSection<TDest, TDest> AddTransform(Transform<TDest, TDest> transform)
		{
			var nextSection = new PipeSection<TDest, TDest>(transform);
			_listeners.Add(nextSection);
			return nextSection;
		}

		protected void SendDownstream(TDest result)
		{
			foreach (var listener in _listeners) { listener.Consume(result); }
		}

		[NotNull] private readonly List<PipeHead<TDest>> _listeners = new List<PipeHead<TDest>>();
	}
}
