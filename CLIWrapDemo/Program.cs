using CliWrap;
using CliWrap.Buffered;
using CLIWrapDemo.Example;

var dotnet = await Cli.Wrap("dotnet").WithArguments(new[] { "--version" }).ExecuteBufferedAsync();

Console.WriteLine(dotnet.StandardOutput);

var client = new GitClient(@"C:\Adorama\adorama\");

var result = await client.GetBranches();
Console.WriteLine(result);
