using ApprovalTests.Maintenance;
using NUnit.Framework;

namespace Gibberish.Tests.ZzTestHelpers
{
	[TestFixture]
	public class TestsShouldBeClean
	{
		[Test]
		public void should_be_no_extra_approvals_checked_in()
		{
			ApprovalMaintenance.VerifyNoAbandonedFiles();
		}
	}
}
