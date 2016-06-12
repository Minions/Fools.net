using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ide.Tests
{
    [TestClass]
    public class FormatterTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var subject = new CaseSwappingFormatter();
            subject.Format("~ABcd~")
                .Should().Be("~abCD~");
        }
    }
}
