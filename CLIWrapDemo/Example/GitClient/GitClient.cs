using CliWrap;
using CliWrap.Buffered;

namespace CLIWrapDemo.Example.GitClient;

public enum GitCommand
{
    init = 1,
    status,
    add,
    commit,
    push,
    pull,
    version,
    checkout,
    clone,
    branch,
    end
}

public class GitClient : IGitClient
{

    private const string GIT = "git";
    private const string BRANCH_ALL = "-a";
    private const string ADD_ALL = "*";
    private const char SPACE = ' ';

    private readonly string workingPath;

    private readonly string commands = $"[1] - init\t[2] - status\t[3] - add\t[4] - commit\t[5] - push\t[6] - pull\n" +
                                       $"[7] - version\t[8] - checkout\t[9] - clone\t[10] - branches\t[11] - end";

    public GitClient()
    {
        Console.Write("Enter working path: ");
        workingPath = Console.ReadLine();

        bool stop = false;

        while (!stop)
        {
            try
            {
                Console.WriteLine(commands);
                Console.Write("Choose git operation: ");
                var command = (GitCommand)Int32.Parse(Console.ReadLine());

                if(command == GitCommand.end)
                {
                    stop = true;
                }
                else
                {
                    var output = Execute(command).Result;
                    Console.WriteLine('\n' + output);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }


    private async Task<string> Execute(GitCommand command) => command switch
        {
            GitCommand.init => await Init(),
            GitCommand.add => await Add(),
            GitCommand.commit => await Commit(),
            GitCommand.push => await Push(),
            GitCommand.pull => await Pull(),
            GitCommand.version => await GetVersion(),
            GitCommand.clone => await Clone(),
            GitCommand.checkout => await ChangeBranche(),
            GitCommand.branch => await GetBranches(),
            GitCommand.status => await Status(),
            _ => "Wrong type of git operation!"
        };


    private async Task<string> CommonMethod(params string[] args)
    {
        try
        {            
            var result = await Cli.Wrap(GIT)
                .WithArguments(string.Join(SPACE, args))
                .WithWorkingDirectory(workingPath)
                .WithValidation(CommandResultValidation.ZeroExitCode)
                .ExecuteBufferedAsync();

            return result.StandardOutput;
        }
        catch (Exception ex)
        {
            Console.WriteLine('\n' + ex.Message);
            return string.Empty;
        }
    }


    #region MAIN_COMMANDS

    public async Task<string> ChangeBranche()
    {
        var branches = Execute(GitCommand.branch).Result;
        Console.WriteLine('\n' + branches);
        Console.Write("\nEnter branch name: ");
        var branch = Console.ReadLine();
        return await CommonMethod(GitCommand.checkout.ToString(), branch);
    }

    public async Task<string> Clone()
    {
        Console.Write("\nEnter source: ");
        var source = Console.ReadLine();
        return await CommonMethod(GitCommand.clone.ToString(), source);
    }

    public async Task<string> Commit()
    {
        Console.Write("\nEnter commit message: ");
        var message = Console.ReadLine();
        return await CommonMethod(string.Format(GitCommand.commit.ToString(), string.IsNullOrEmpty(message) ? "default message" : message));
    }

    public async Task<string> Add() => await CommonMethod(GitCommand.add.ToString(), ADD_ALL);

    public async Task<string> GetBranches() => await CommonMethod(GitCommand.branch.ToString(), BRANCH_ALL);

    public async Task<string> GetVersion() => await CommonMethod(GitCommand.version.ToString());

    public async Task<string> Init() => await CommonMethod(GitCommand.init.ToString());

    public async Task<string> Status() => await CommonMethod(GitCommand.status.ToString());

    public async Task<string> Pull() => await CommonMethod(GitCommand.pull.ToString());

    public async Task<string> Push() => await CommonMethod(GitCommand.push.ToString());

    #endregion
}
