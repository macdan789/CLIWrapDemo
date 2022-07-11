using CliWrap;
using CliWrap.Buffered;
using CLIWrapDemo.Example.GitClient;

//-----------Simple-Example--------------

var dotnet = await Cli.Wrap("dotnet")
    .WithArguments(new[] { "--version" })
    .ExecuteBufferedAsync();

Console.WriteLine(dotnet.StandardOutput);

//----------Simple-GitClient--------------

var gitClient = new GitClient();

//--------Execute-Powershell-file---------

var powershellResult = await Cli.Wrap("powershell")
    .WithArguments(new[] { @"...\Example\Powershell\Demo.ps1" })
    .ExecuteBufferedAsync();

Console.WriteLine(powershellResult.StandardOutput);
