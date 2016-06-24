using System.IO;
using System.Threading.Tasks;
using Lair.StringExtensions;

namespace Lair
{
    public class LocalFileSystemDocumentStore : IDocumentStore
    {
        static readonly string[] ExtensionFilters =
        {
            "Fools files (*.fool)|*.fool",
            "Minions files (*.minions)|*.minions",
            "All files (*.*)|*.*",
        };

        public async Task<IDocument> Open()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".fool",
                Filter = ExtensionFilters.JoinString("|")
            };
            var result = openFileDialog.ShowDialog();
            if (result != true) return null;

            return new Document(new FileInfo(openFileDialog.FileName));
        }

        public async Task Save(IDocument document)
        {
            var localFileSystemDocument = (Document) document;
            File.WriteAllText(localFileSystemDocument.FileInfo.FullName, localFileSystemDocument.Contents);
        }

        public class Document : IDocument
        {
            public readonly FileInfo FileInfo;

            public Document(FileInfo fileInfo)
            {
                FileInfo = fileInfo;
                Contents = File.ReadAllText(FileInfo.FullName);
            }

            public string Contents { get; set; }
        }

    }
}