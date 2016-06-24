using FluentAssertions;
using NUnit.Framework;

namespace Lair.Tests
{
	[TestFixture]
	public class Formatters
	{
		[Test]
		public void SwapCase()
		{
			var subject = new CaseSwappingFormatter();
			subject.Format("~ABcd~")
				.Should()
				.Be("~abCD~");
		}
	}
}
