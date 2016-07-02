using System;

namespace Lair
{
	public static class Program
	{
		[STAThread]
		public static void Main()
		{
			var main = new Model(new CaseSwappingFormatter().Format, new LocalFileSystemDocumentStore());
			var mainView = new MainWindow
			{
				DataContext = main.ViewModel
			};
			mainView.ShowDialog();
		}
	}
}
