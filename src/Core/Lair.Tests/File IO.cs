using System.Threading.Tasks;
using FluentAssertions;
using Lair.Tests.ZzTestHelpers;
using NUnit.Framework;

namespace Lair.Tests
{
	[TestFixture]
	public class File_IO
	{
		[Test]
		public async Task OpenFillsInTheCode()
		{
			var subject = new Model(
				null,
				new InMemorySingleDocumentStore
				{
					Contents = "Look at me! I am a Minion!"
				});
			subject.ViewModel.Code.Text.Should()
				.BeEmpty();
			await subject.ReplaceCurrentCodeWithFileContents();
			subject.ViewModel.Code.Text.Should()
				.Be("Look at me! I am a Minion!");
		}

		[Test]
		public async Task OpenLooksForBugs()
		{
			var subject = new Model(
				null,
				new InMemorySingleDocumentStore
				{
					Contents = TestData.BugFreeFoolsCode
				});
			subject.ViewModel.Errors.Should()
				.BeEmpty();
			await subject.ReplaceCurrentCodeWithFileContents();
			subject.ViewModel.Errors.Should()
				.Be(TestData.BugsFoundInBugFreeFoolsCode);
		}

		[Test]
		public async Task SaveWritesOutTheCode()
		{
			var inMemorySingleDocumentStore = new InMemorySingleDocumentStore();
			var subject = new Model(null, inMemorySingleDocumentStore);
			await subject.ReplaceCurrentCodeWithFileContents();
			subject.ViewModel.Code.Text = "Do you take me for a fool?";
			await subject.SaveCurrentCodeToFile();
			inMemorySingleDocumentStore.Contents.Should()
				.Be("Do you take me for a fool?");
		}
	}
}
