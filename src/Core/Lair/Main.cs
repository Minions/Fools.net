using System;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using static System.Char;

namespace Lair
{
    public class Main
    {
        readonly Func<string, string> _formatter;
        readonly IDocumentStore _documentStore;
        [CanBeNull]
        IDocument _currentDocument;

        public Main(Func<string, string> formatter, IDocumentStore documentStore)
        {
            _formatter = formatter;
            _documentStore = documentStore;
            ViewModel = new MainViewModel
            {
                FormatAll = {On = OnFormatAll},
                Open = {On = OnOpen},
                Save = {On = OnSave}
            };
        }

        public MainViewModel ViewModel { get; }

        public async Task OnSave()
        {
            _currentDocument.Contents = ViewModel.Code;
            await _documentStore.Save(_currentDocument);
        }

        public async Task OnOpen()
        {
            _currentDocument = await _documentStore.Open();
            ViewModel.Code = _currentDocument.Contents;
        }

        public async Task OnFormatAll()
        {
            ViewModel.Code = _formatter(ViewModel.Code);
        }
    }
}