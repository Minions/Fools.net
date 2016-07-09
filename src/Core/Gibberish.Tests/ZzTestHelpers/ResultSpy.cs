using System.Collections.Generic;
using FluentAssertions;

namespace Gibberish.Tests.ZzTestHelpers
{
	public class ResultSpy<T>
	{
		public void Receive(T val)
		{
			_values.Add(val);
		}

		public void ShouldContain(params T[] expected)
		{
			_values.Should()
				.BeEquivalentTo(expected);
		}

		private readonly List<T> _values = new List<T>();
	}
}