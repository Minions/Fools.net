using System.Linq;
using JetBrains.Annotations;

namespace Gibberish.Parsing.TransformPipeline
{
	public class PipeSection<TSrc, TDest> : PipeTail<TDest>, PipeHead<TSrc>
	{
		public PipeSection([NotNull] Transform<TSrc, TDest> transform)
		{
			_transform = transform;
		}

		public PipeSection<TDest, TDest> AddTransformSequence(params Transform<TDest, TDest>[] transforms)
		{
			var firstStep = AddTransform(transforms.First());
			return transforms.Skip(1)
				.Aggregate(firstStep, (current, transform) => current.AddTransform(transform));
		}

		[NotNull] private readonly Transform<TSrc, TDest> _transform;

		public void Consume(TSrc val)
		{
			SendDownstream(_transform.Transform(val));
		}
	}
}
