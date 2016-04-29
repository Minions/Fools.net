using System;

namespace Gibberish.Parsing
{
	internal class UnfixableError : Exception
	{
		public UnfixableError(string messageDetails) : base("This is a compiler bug. Please report it. Details: " + messageDetails) {}
	}
}