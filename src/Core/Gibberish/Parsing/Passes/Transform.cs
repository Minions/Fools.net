using JetBrains.Annotations;

namespace Gibberish.Parsing.Passes
{
	public interface Transform<in TInput, out TOutput>
	{
		[NotNull]
		TOutput Transform([NotNull] TInput input);
	}
}
