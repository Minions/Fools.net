using System;
using System.Threading.Tasks;
using ApprovalTests.Wpf;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ide.Tests
{
    [TestClass]
    public class MainWindowTests
    {
        [TestMethod]
        [STAThread]
        public void MainWindowBindsToViewModelWithoutError()
        {
            MainWindow window = null;
            WpfBindingsAssert.BindsWithoutError(new MainViewModel(), () => window = new MainWindow());
            window.Close();
        }

        [TestMethod]
        public void MainCreatesAViewModel()
        {
            var subject = new Main(null, null);
            subject.ViewModel.Should().NotBeNull();
        }

        [TestMethod]
        public async Task OpenFillsInTheCode()
        {
            var subject = new Main(null, new InMemorySingleDocumentStore() {Contents = "Look at me! I am a Minion!"});
            await subject.OnOpen();
            subject.ViewModel.Code.Should().Be("Look at me! I am a Minion!");
        }

        [TestMethod]
        public async Task SaveWritesOutTheCode()
        {
            var inMemorySingleDocumentStore = new InMemorySingleDocumentStore();
            var subject = new Main(null, inMemorySingleDocumentStore);
            await subject.OnOpen();
            subject.ViewModel.Code = "Do you take me for a fool?";
            await subject.OnSave();
            inMemorySingleDocumentStore.Contents.Should().Be("Do you take me for a fool?");
        }

        class InMemorySingleDocumentStore : IDocumentStore
        {
            public string Contents;

            public async Task<IDocument> Open()
            {
                return new Document() {Contents = Contents};
            }

            class Document : IDocument
            {
                public string Contents { get; set; }
            }

            public async Task Save(IDocument document)
            {
                this.Contents = document.Contents;
            }
        }


        [TestMethod]
        public async Task Format()
        {
            var subject = new Main(_ => "blah blah", null);
            await subject.OnFormatAll();
            subject.ViewModel.Code.Should().Be("blah blah");
        }
    }
}