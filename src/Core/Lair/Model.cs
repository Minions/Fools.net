using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gibberish.AST._1_Bare;
using Gibberish.Parsing;
using JetBrains.Annotations;

namespace Lair
{
	public class Model
	{
		public Model(Func<string, string> formatter, IDocumentStore documentStore)
		{
			_pipelineStart = LanguageTools.Pipeline()
				.WithResultListener(_HandleResults)
				.Build();
			_formatter = formatter;
			_documentStore = documentStore;
			ViewModel = new MainViewModel
			{
				FormatAll = {
					On = OnFormatAll
				},
				Open = {
					On = OnOpen
				},
				Save = {
					On = OnSave
				}
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
			UpdateErrors();
		}

		public async Task OnFormatAll()
		{
			ViewModel.Code = _formatter(ViewModel.Code);
			UpdateErrors();
		}

		private void _HandleResults(List<LanguageConstruct> results)
		{
			var errors = new ErrorFinder();
			results.ForEach(block => block.Accept(errors));
			ViewModel.Errors = errors.Errors.Count > 0 ? string.Join("\r\n\r\n", errors.Errors) : "Zaro Boogs Foond.\r\n\r\nYou're all good, boss!";
		}

		private void UpdateErrors()
		{
			if (ViewModel.Code != null) _pipelineStart.Analyze(ViewModel.Code);
		}

		private readonly IDocumentStore _documentStore;
		private readonly Func<string, string> _formatter;
		private readonly LanguagePipelineStart _pipelineStart;
		[CanBeNull] private IDocument _currentDocument;
	}
}
