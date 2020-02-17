# Cslint
## Rationale

Performs the following checks C# source code files (*.cs). 
- Line feeds without carriage return
- BOM other than UTF-8

These are things normally not checked or enforced in any way by StyleCop or Visual Studio or Visual Studio Code or even the Roslyn compiler. Furthermore, these are kind of tricky to check in Shell, Bash, or Powershell, and quite frequently produces scripts
that doesn't favor readability. Therefore a simple console app is suggested and this can in turn be referenced as a build step in CI-environment.

## Example usage

    dotnet run poop path=C:\somedevelopment\somework

Or if included in the repo as a binary or an agent that has the latest dotnet core sdk. 

    dotnet run

More complex use cases. Use regex to pipe out only errors. 

	dotnet run path=..\..\someproject\ | Select-String -Pattern "^EE"
	EE      No utf8 BOM: C:\sources\someproject\src\snark\Program.cs
	EE      No utf8 BOM: C:\Humany\someproject\src\snark\Startup.cs
	EE      Matches LF: C:\Humany\someproject\src\snark\ConfigStuffs.cs

## Building 

    dotnet publish /p:PublishSingleFile=true --self-contained --no-restore --runtime linux-x64 -c Release
    dotnet publish --framework netcoreapp3.0 --runtime osx.10.11-x64
    dotnet publish --framework netcoreapp3.0 --self-contained --runtime win-x64 -c Release /p:PublishSingleFile=true

## Use case

In Azure DevOps it could be handy to run it in a build pipeline for pull requests. Including only a LICENCE and the
binary for the particular agent platform.

## Example output
```pwsh
C:/somedir/cslint
||      Matches CRLF: C:\somedir\cslint\Cslint\ArgsParser.cs
||      Utf8 BOM OK: C:\somedir\cslint\Cslint\ArgsParser.cs
||      Matches CRLF: C:\somedir\cslint\Cslint\CheckerBase.cs
||      Utf8 BOM OK: C:\somedir\cslint\Cslint\CheckerBase.cs
||      Matches CRLF: C:\somedir\cslint\Cslint\GitProcess.cs
||      Utf8 BOM OK: C:\somedir\cslint\Cslint\GitProcess.cs
||      Matches CRLF: C:\somedir\cslint\Cslint\IChecker.cs
||      Utf8 BOM OK: C:\somedir\cslint\Cslint\IChecker.cs
||      Matches CRLF: C:\somedir\cslint\Cslint\Lf.cs
||      Utf8 BOM OK: C:\somedir\cslint\Cslint\Lf.cs
||      Matches CRLF: C:\somedir\cslint\Cslint\Lint.cs
||      Utf8 BOM OK: C:\somedir\cslint\Cslint\Lint.cs
||      Matches CRLF: C:\somedir\cslint\Cslint\Program.cs
||      Utf8 BOM OK: C:\somedir\cslint\Cslint\Program.cs
||      Matches CRLF: C:\somedir\cslint\Cslint\Utf8Bom.cs
||      Utf8 BOM OK: C:\somedir\cslint\Cslint\Utf8Bom.cs
||      Matches CRLF: C:\somedir\cslint\Cslint\obj\Debug\netcoreapp2.1\LineFeedLinter.AssemblyInfo.cs
||      Matches CRLF: C:\somedir\cslint\Cslint\obj\Debug\netcoreapp3.0\Cslint.AssemblyInfo.cs
||      Matches CRLF: C:\somedir\cslint\Cslint\obj\Debug\netcoreapp3.0\LineFeedLinter.AssemblyInfo.cs
||      Matches CRLF: C:\somedir\cslint\CslintTests\ArgumentParserTests.cs
||      Utf8 BOM OK: C:\somedir\cslint\CslintTests\ArgumentParserTests.cs
||      Matches CRLF: C:\somedir\cslint\CslintTests\ProgramTests.cs
||      Utf8 BOM OK: C:\somedir\cslint\CslintTests\ProgramTests.cs
||      Matches CRLF: C:\somedir\cslint\CslintTests\obj\Debug\netcoreapp2.1\LineFeedLinterTests.AssemblyInfo.cs
||      Matches CRLF: C:\somedir\cslint\CslintTests\obj\Debug\netcoreapp3.0\CslintTests.AssemblyInfo.cs
||      Matches CRLF: C:\somedir\cslint\CslintTests\obj\Debug\netcoreapp3.0\CslintTests.Program.cs
||      Matches CRLF: C:\somedir\cslint\CslintTests\obj\Debug\netcoreapp3.0\LineFeedLinterTests.AssemblyInfo.cs
||      Matches CRLF: C:\somedir\cslint\CslintTests\obj\Debug\netcoreapp3.0\LineFeedLinterTests.Program.cs
```

## Prospects
- Add intent as per gist: https://gist.github.com/jmrnilsson/540c77c86a3f9a38b08e589170734666
- Add autofix option for some of the issues.
