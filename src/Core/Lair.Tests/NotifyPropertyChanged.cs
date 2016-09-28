using Lair.Tests.ZzTestHelpers;
using NUnit.Framework;

namespace Lair.Tests
{
	[TestFixture]
	public class NotifyPropertyChanged
	{
		[Test]
		public void ValueRaisesPropertyChanged()
		{
			var subject = new TestViewModel();
			subject.MonitoringEvents(_ => _.Value = "foo")
				.ShouldRaisePropertyChangeFor(_ => _.Value);
		}

		private class TestViewModel : Lair.NotifyPropertyChanged
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
