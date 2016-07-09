namespace Lair
{
	public class MainViewModel : NotifyPropertyChanged
	{
		public SimpleCommand Open { get; } = new SimpleCommand();
		public SimpleCommand Save { get; } = new SimpleCommand();
		public SimpleCommand FormatAll { get; } = new SimpleCommand();
		public SimpleCommand CodeChanged { get; } = new SimpleCommand();

		public string Code
		{
			get { return _code; }
			set
			{
				if (string.Equals(_code, value)) return;
				_code = value;
				RaisePropertyChanged();
				if (CodeChanged.CanExecute(value)) CodeChanged.Execute(value);
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
		private string _code = string.Empty;
		private string _errors = string.Empty;
	}
}
