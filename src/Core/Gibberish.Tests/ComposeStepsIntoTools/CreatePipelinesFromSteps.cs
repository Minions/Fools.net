using Gibberish.Parsing.TransformPipeline;
using Gibberish.Tests.ZzTestHelpers;
using NUnit.Framework;

namespace Gibberish.Tests.ComposeStepsIntoTools
{
	[TestFixture]
	public class CreatePipelinesFromSteps
	{
		[Test]
		public void SingleStepPipelineShouldExecuteStep()
		{
			var testSubject = new PipeSection<int, int>(new _AddOne());
			var inputPlusOne = _ListenTo(testSubject);
			testSubject.Consume(1);
			inputPlusOne.ShouldContain(2);
		}

		[Test]
		public void ExecutingAStepMultipleTimesShouldResultInMultipleCallsToTheListener()
		{
			var testSubject = new PipeSection<int, int>(new _AddOne());
			var inputPlusOne = _ListenTo(testSubject);
			testSubject.Consume(1);
			testSubject.Consume(4);
			inputPlusOne.ShouldContain(2, 5);
		}

		[Test]
		public void BranchingPipelineShouldExecuteAllBranches()
		{
			var testSubject = new PipeSection<int, int>(new _AddOne());
			var addThree = testSubject.AddTransformSequence(new _AddOne(), new _AddOne());
			var addTwo = testSubject.AddTransformSequence(new _AddOne());
			var inputPlusOne = _ListenTo(testSubject);
			var inputPlusTwo = _ListenTo(addTwo);
			var inputPlusThree = _ListenTo(addThree);
			testSubject.Consume(1);
			inputPlusOne.ShouldContain(2);
			inputPlusTwo.ShouldContain(3);
			inputPlusThree.ShouldContain(4);
		}

		private static ResultSpy<int> _ListenTo(PipeSection<int, int> testSubject)
		{
			var inputPlusOne = new ResultSpy<int>();
			testSubject.AddListener(inputPlusOne.Receive);
			return inputPlusOne;
		}

		public class _AddOne : Transform<int, int>
		{
			public int Transform(int input)
			{
				return input + 1;
			}
		}
	}
}
