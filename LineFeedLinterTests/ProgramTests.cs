using LineFeedLinter;
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace LineFeedLinterTests
{
    public class GitProcessTests
    {
        GitProcess gitProcess = new GitProcess();

        [Fact]
        public void TopLevelFound()
        {
            var actual = gitProcess.TopLevel();
            Match match = Regex.Match(actual, "LineFeedLinter$");
            Assert.Equal("LineFeedLinter", match.Value);
        }
    }
}
