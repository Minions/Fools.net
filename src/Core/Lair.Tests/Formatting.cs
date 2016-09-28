using System.Threading.Tasks;
using FluentAssertions;
using Lair.Tests.ZzTestHelpers;
using NUnit.Framework;

namespace Lair.Tests
{
	[TestFixture]
	public class Formatting
	{
		[Test]
		public void SwapCase()
		{
			var subject = new CaseSwappingFormatter();
			subject.Format("~ABcd~")
				.Should()
				.Be("~abCD~");
		}

		[Test]
		public async Task FormatShouldReplaceCodeWithFormatterResult()
		{
			var subject = new Model(_ => TestData.BugFreeFoolsCode, null);
			subject.ViewModel.Code.Text.Should()
				.BeEmpty();
			await subject.AutoformatCurrentCode();
			subject.ViewModel.Code.Text.Should()
				.Be(TestData.BugFreeFoolsCode);
		}

		[Test]
		public async Task FormatShouldReevaluateForBugsAfterResult()
		{
			var subject = new Model(_ => TestData.BugFreeFoolsCode, null);
			subject.ViewModel.Errors.Should()
				.BeEmpty();
			await subject.AutoformatCurrentCode();
			subject.ViewModel.Errors.Should()
				.Be(TestData.BugsFoundInBugFreeFoolsCode);
		}
	}
}
