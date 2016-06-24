namespace Lair
{
    public class MainViewModel : NotifyPropertyChanged
    {
        string _code;
        public SimpleCommand Open { get; } = new SimpleCommand();
        public SimpleCommand Save { get; } = new SimpleCommand();
        public SimpleCommand FormatAll { get; } = new SimpleCommand();

        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                RaisePropertyChanged();
            }
        }
    }
}