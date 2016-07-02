using System.IO;
using System.Threading.Tasks;
using Lair.StringExtensions;
using Microsoft.Win32;

namespace Lair
{
	public class LocalFileSystemDocumentStore : IDocumentStore
	{
		public class Document : IDocument
		{
			public Document(FileInfo fileInfo)
			{
				FileInfo = fileInfo;
				Contents = File.ReadAllText(FileInfo.FullName);
			}

			public readonly FileInfo FileInfo;

			public string Contents { get; set; }
		}

		public async Task<IDocument> Open()
		{
			var openFileDialog = new OpenFileDialog
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

		private static readonly string[] ExtensionFilters = {
			"Fools files (*.fool)|*.fool",
			"Minions files (*.minions)|*.minions",
			"All files (*.*)|*.*"
		};
	}
}
