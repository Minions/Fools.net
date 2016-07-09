using ICSharpCode.AvalonEdit.Document;

namespace Lair
{
	public class MainViewModel : NotifyPropertyChanged
	{
		public MainViewModel()
		{
			_code = new TextDocument(string.Empty);
			ListenToCodeDocForTextChanges();
		}

		public SimpleCommand Open { get; } = new SimpleCommand();
		public SimpleCommand Save { get; } = new SimpleCommand();
		public SimpleCommand FormatAll { get; } = new SimpleCommand();
		public SimpleCommand CodeChanged { get; } = new SimpleCommand();

		public TextDocument Code
		{
			get { return _code; }
			set
			{
				if (_code == value) return;
				_code = value;
				RaisePropertyChanged();
				if (CodeChanged.CanExecute(value)) CodeChanged.Execute(value);
				ListenToCodeDocForTextChanges();
			}
		}

		public string Errors
		{
			get { return _errors; }
			set
			{
				_errors = value;
				RaisePropertyChanged();
			}
		}

		private void ListenToCodeDocForTextChanges()
		{
			_code.TextChanged += (sender, args) =>
			{
				if (CodeChanged.CanExecute(_code)) CodeChanged.Execute(_code);
			};
		}

		private TextDocument _code;
		private string _errors = string.Empty;
	}
}
