using System.Threading;
using System.Threading.Tasks;
using ApprovalTests.Wpf;
using FluentAssertions;
using NUnit.Framework;

namespace Lair.Tests
{
	[TestFixture]
	public class MainWindowBindsToUi
	{
		[Test, Apartment(ApartmentState.STA)]
		public void MainWindowBindsToViewModelWithoutError()
		{
			MainWindow window = null;
			WpfBindingsAssert.BindsWithoutError(new MainViewModel(), () => window = new MainWindow());
			window.Close();
		}

		[Test]
		public void MainCreatesAViewModel()
		{
			var subject = new Model(null, null);
			subject.ViewModel.Should()
				.NotBeNull();
		}

		[Test]
		public async Task OpenFillsInTheCode()
		{
			var subject = new Model(
				null,
				new InMemorySingleDocumentStore
				{
					Contents = "Look at me! I am a Minion!"
				});
			subject.ViewModel.Code.Should()
				.BeEmpty();
			await subject.OnOpen();
			subject.ViewModel.Code.Should()
				.Be("Look at me! I am a Minion!");
		}

		[Test]
		public async Task OpenLooksForBugs()
		{
			var subject = new Model(
				null,
				new InMemorySingleDocumentStore
				{
					Contents = BugFreeFoolsCode
				});
			subject.ViewModel.Errors.Should()
				.BeEmpty();
			await subject.OnOpen();
			subject.ViewModel.Errors.Should()
				.Be(BugsFoundInBugFreeFoolsCode);
		}

		[Test]
		public async Task SaveWritesOutTheCode()
		{
			var inMemorySingleDocumentStore = new InMemorySingleDocumentStore();
			var subject = new Model(null, inMemorySingleDocumentStore);
			await subject.OnOpen();
			subject.ViewModel.Code = "Do you take me for a fool?";
			await subject.OnSave();
			inMemorySingleDocumentStore.Contents.Should()
				.Be("Do you take me for a fool?");
		}

		[Test]
		public async Task FormatShouldReplaceCodeWithFormatterResult()
		{
			var subject = new Model(_ => BugFreeFoolsCode, null);
			subject.ViewModel.Code.Should()
				.BeEmpty();
			await subject.OnFormatAll();
			subject.ViewModel.Code.Should()
				.Be(BugFreeFoolsCode);
		}

		[Test]
		public async Task FormatShouldReevaluateForBugsAfterResult()
		{
			var subject = new Model(_ => BugFreeFoolsCode, null);
			subject.ViewModel.Errors.Should()
				.BeEmpty();
			await subject.OnFormatAll();
			subject.ViewModel.Errors.Should()
				.Be(BugsFoundInBugFreeFoolsCode);
		}

		private const string BugFreeFoolsCode = "use language fools\r\nthat's not right\r\n";
		private const string BugsFoundInBugFreeFoolsCode = "Zaro Boogs Foond.\r\n\r\nYou're all good, boss!";

		private class InMemorySingleDocumentStore : IDocumentStore
		{
			public string Contents;

			public async Task<IDocument> Open()
			{
				return new Document
				{
					Contents = Contents
				};
			}

			public async Task Save(IDocument document)
			{
				Contents = document.Contents;
			}

			private class Document : IDocument
			{
				public string Contents { get; set; }
			}
		}
	}
}
