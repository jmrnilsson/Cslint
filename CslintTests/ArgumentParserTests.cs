using Cslint;
using Shouldly;
using System.IO;
using Xunit;

namespace CslintTests
{
    public class ArgsParserTests
    {
        ArgsParser argsParser = new ArgsParser();

        [Fact]
        public void ParseCitedArguments()
        {
            string[] args = new[] { "path=C:\\" };
            DirectoryInfo actual = argsParser.Parse(args);
            actual.FullName.ShouldContain("C:");
        }
    }
}
