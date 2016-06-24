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
			var subject = new Main(null, null);
			subject.ViewModel.Should()
				.NotBeNull();
		}

		[Test]
		public async Task OpenFillsInTheCode()
		{
			var subject = new Main(
				null,
				new InMemorySingleDocumentStore
				{
					Contents = "Look at me! I am a Minion!"
				});
			await subject.OnOpen();
			subject.ViewModel.Code.Should()
				.Be("Look at me! I am a Minion!");
		}

		[Test]
		public async Task SaveWritesOutTheCode()
		{
			var inMemorySingleDocumentStore = new InMemorySingleDocumentStore();
			var subject = new Main(null, inMemorySingleDocumentStore);
			await subject.OnOpen();
			subject.ViewModel.Code = "Do you take me for a fool?";
			await subject.OnSave();
			inMemorySingleDocumentStore.Contents.Should()
				.Be("Do you take me for a fool?");
		}

		[Test]
		public async Task Format()
		{
			var subject = new Main(_ => "blah blah", null);
			await subject.OnFormatAll();
			subject.ViewModel.Code.Should()
				.Be("blah blah");
		}

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
