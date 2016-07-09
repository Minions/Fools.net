namespace Gibberish.Parsing.TransformPipeline
{
	public interface PipeHead<TSrc> {
		void Consume(TSrc val);
	}
}
