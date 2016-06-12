using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Ide
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs _)
        {
            var main = new Main(new CaseSwappingFormatter().Format, new LocalFileSystemDocumentStore());
            var mainView = new MainWindow {DataContext = main.ViewModel};
            mainView.Show();
        }
    }
}