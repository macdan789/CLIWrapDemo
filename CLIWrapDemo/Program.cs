using CliWrap;
using CliWrap.Buffered;

var dotnet = await Cli.Wrap("dotnet").WithArguments("--version").ExecuteBufferedAsync();

Console.WriteLine(dotnet.StandardOutput);
