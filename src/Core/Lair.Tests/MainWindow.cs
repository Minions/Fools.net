using System.Threading;
using System.Threading.Tasks;
using ApprovalTests.Wpf;
using FluentAssertions;
using NUnit.Framework;

namespace Lair.Tests
{
	[TestFixture]
	public class MainWindow
	{
		[Test, Apartment(ApartmentState.STA)]
		public void BindsToViewModelWithoutError()
		{
			Lair.MainWindow window = null;
			WpfBindingsAssert.BindsWithoutError(new MainViewModel(), () => window = new Lair.MainWindow());
			window.Close();
		}

		[Test]
		public void ModelCreatesAViewModel()
		{
			var subject = new Model(null, null);
			subject.ViewModel.Should()
				.NotBeNull();
		}
	}
}
