using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ide.Tests
{
    [TestClass]
    public class NotifyPropertyChangedTests
    {
        [TestMethod]
        public void RaisesPropertyChanged()
        {
            var subject = new TestViewModel();
            subject.MonitoringEvents(
                _ => _.Value = "foo"
                ).ShouldRaisePropertyChangeFor(_ => _.Value);
        }

        class TestViewModel : NotifyPropertyChanged
        {
            string _value;

            public string Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}