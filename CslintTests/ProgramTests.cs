using Cslint;
using System.Text.RegularExpressions;
using Xunit;

namespace CslintTests
{
	public class GitProcessTests
	{
		GitProcess gitProcess = new GitProcess();

		[Fact]
		public void TopLevelFound()
		{
			var actual = gitProcess.TopLevel();
			Match match = Regex.Match(actual, "[Cc]slint");
			Assert.Contains("slint", match.Value);
		}
	}
}
