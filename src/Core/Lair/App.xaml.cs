using System.Windows;

namespace Lair
{
	public partial class App
	{
		protected override void OnStartup(StartupEventArgs _)
		{
			var main = new Main(new CaseSwappingFormatter().Format, new LocalFileSystemDocumentStore());
			var mainView = new MainWindow
			{
				DataContext = main.ViewModel
			};
			mainView.Show();
		}
	}
}
