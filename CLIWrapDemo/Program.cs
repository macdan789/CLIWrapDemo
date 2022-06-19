using CliWrap;
using CliWrap.Buffered;
using CLIWrapDemo.Example;

var dotnet = await Cli.Wrap("dotnet")
    .WithArguments(new[] { "--version" })
    .ExecuteBufferedAsync();

Console.WriteLine(dotnet.StandardOutput);

//----------Simple-GitClient--------------

var path = @"your_working_directory";
IGitClient client = new GitClient(path);

var addResult = await client.Add();
Console.WriteLine(addResult);

var commitResult = await client.Commit("commit via Git Client");
Console.WriteLine(commitResult);

var pushResult = await client.Push();
Console.WriteLine(pushResult);
