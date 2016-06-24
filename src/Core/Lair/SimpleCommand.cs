using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lair
{
    public class SimpleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };
        public Func<Task> On = async delegate { };
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => On();
    }
}