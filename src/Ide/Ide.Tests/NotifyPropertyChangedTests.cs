using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ide.Tests
{
    [TestClass]
    public class NotifyPropertyChangedTests
    {
        [TestMethod]
        public void RaisesPropertyChanged()
        {
            TestViewModel subject = new TestViewModel();
            subject.MonitorEvents();
            subject.Value = "foo";
            subject.ShouldRaisePropertyChangeFor(_ => _.Value);
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