using NUnit.Framework;

namespace Lair.Tests
{
	[TestFixture]
	public class ViewModelPropogatesChanges
	{
		[Test]
		public void ValueRaisesPropertyChanged()
		{
			var subject = new TestViewModel();
			subject.MonitoringEvents(_ => _.Value = "foo")
				.ShouldRaisePropertyChangeFor(_ => _.Value);
		}

		private class TestViewModel : NotifyPropertyChanged
		{
			public string Value
			{
				get { return _value; }
				set
				{
					_value = value;
					RaisePropertyChanged();
				}
			}
			private string _value;
		}
	}
}
