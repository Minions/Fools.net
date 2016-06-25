using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lair
{
	public class SimpleCommand : ICommand
	{
		public Func<Task> On = async delegate { };
		public event EventHandler CanExecuteChanged = delegate { };

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter) => On();
	}
}
