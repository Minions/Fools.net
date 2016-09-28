using System;
using System.Threading.Tasks;

namespace Lair.Tests.ZzTestHelpers
{
	internal class InMemorySingleDocumentStore : IDocumentStore
	{
		public string Contents = String.Empty;

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
			public string Contents { get; set; } = String.Empty;
		}
	}
}
