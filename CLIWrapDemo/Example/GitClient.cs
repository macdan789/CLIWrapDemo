using CliWrap;
using CliWrap.Buffered;
using System.Text;

namespace CLIWrapDemo.Example;

public enum GitCommand
{
    INIT = 1,
    STATUS,
    ADD,
    COMMIT,
    PUSH,
    PULL,
    VERSION,
    CHECKOUT,
    CLONE,
    CURRENT,
    BRANCHES,
    END
}

public class GitClient : IGitClient
{
    #region CONSTANTS

    private const string GIT_COMMAND = "git";
    private const string GIT_STATUS = "status";
    private const string GIT_COMMIT_COMMAND = "commit -m \"{0}\"";
    private const string GIT_ADD_COMMAND = "add *";
    private const string GIT_INIT_COMMAND = "init";
    private const string GIT_PULL_COMMAND = "pull";
    private const string GIT_PUSH_COMMAND = "push";
    private const string GIT_CLONE_COMMAND = "clone";
    private const string GIT_BRANCH_COMMAND = "branch";
    private const string CURRENT = "--show-current";
    private const string GIT_CHECKOUT_COMMAND = "checkout";
    private const string GIT_VERSION_COMMAND = "--version";
    private readonly string workingPath;
    private readonly string commands = $"[1] - init\t[2] - status\t[3] - add\t[4] - commit\t[5] - push\t[6] - pull" +
        $"\n[7] - version\t[8] - checkout\t[9] - clone\t[10] - current\t[11] - branches\t[12] - end";
   
    #endregion

    private bool _continue = true;

    public GitClient()
    {
        Console.Write("Enter working path: ");
        workingPath = Console.ReadLine();

        while (_continue)
        {
            try
            {
                Console.WriteLine(commands);
                Console.Write("Choose git operation: ");
                var type = (GitCommand)Int32.Parse(Console.ReadLine());
                
                if(type == GitCommand.END)
                {
                    _continue = false;
                }
                else if (type == GitCommand.COMMIT)
                {
                    Console.Write("Commit message: ");
                    var message = Console.ReadLine();
                    Console.WriteLine(GetOperation(type, message).Result);
                }
                else if(type == GitCommand.CLONE)
                {
                    Console.Write("Source to clone: ");
                    var source = Console.ReadLine();
                    Console.WriteLine(GetOperation(type, source).Result);
                }
                else if(type == GitCommand.CHECKOUT)
                {
                    Console.WriteLine(GetOperation(GitCommand.BRANCHES, string.Empty).Result);
                    Console.Write("Enter branch name: ");
                    var branch = Console.ReadLine();
                    Console.WriteLine(GetOperation(type, branch).Result);
                }
                else
                {
                    Console.WriteLine(GetOperation(type, string.Empty).Result);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }


    private async Task<string> GetOperation(GitCommand command, string obj) => command switch
        {
            GitCommand.INIT => await Init(),
            GitCommand.ADD => await Add(),
            GitCommand.COMMIT => await Commit(obj),
            GitCommand.PUSH => await Push(),
            GitCommand.PULL => await Pull(),
            GitCommand.VERSION => await GetVersion(),
            GitCommand.CLONE => await Clone(obj),
            GitCommand.CHECKOUT => await ChangeBranche(obj),
            GitCommand.CURRENT => await GetCurrentBranch(),
            GitCommand.BRANCHES => await GetBranches(),
            GitCommand.STATUS => await Status(),
            _ => "Wrong type of git operation!"
        };


    private async Task<string> CommonMethod(params string[] args)
    {
        try
        {            
            var result = await Cli.Wrap(GIT_COMMAND)
                .WithArguments(string.Join(' ', args))
                .WithWorkingDirectory(workingPath)
                .WithValidation(CommandResultValidation.ZeroExitCode)
                .ExecuteBufferedAsync();

            return result.StandardOutput;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return string.Empty;
        }
    }

    private async Task<string> CommonMethodNotBuffered(params string[] args)
    {
        try
        {
            var outputBuffer = new StringBuilder();

            var result = await Cli.Wrap(GIT_COMMAND)
                .WithArguments(args)
                .WithWorkingDirectory(workingPath)
                .WithValidation(CommandResultValidation.ZeroExitCode)
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(outputBuffer))
                .ExecuteAsync();

            return outputBuffer.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return string.Empty;
        }
    }


    #region MAIN_COMMANDS

    public async Task<string> Add()
    {
        return await CommonMethod(GIT_ADD_COMMAND);
    }

    public async Task<string> ChangeBranche(string branch)
    {
        return await CommonMethod(GIT_CHECKOUT_COMMAND, branch);
    }

    public async Task<string> Clone(string source)
    {
        return await CommonMethod(GIT_CLONE_COMMAND, source);
    }

    public async Task<string> Commit(string message)
    {
        return await CommonMethod(string.Format(GIT_COMMIT_COMMAND, message ?? "default message"));
    }

    public async Task<string> GetBranches()
    {
        return await CommonMethod(GIT_BRANCH_COMMAND);
    }

    public async Task<string> GetCurrentBranch()
    {
        return await CommonMethod(GIT_BRANCH_COMMAND, CURRENT);
    }

    public async Task<string> GetVersion()
    {
        return await CommonMethod(GIT_VERSION_COMMAND);
    }

    public async Task<string> Init()
    {
        return await CommonMethod(GIT_INIT_COMMAND);
    }

    public async Task<string> Status()
    {
        return await CommonMethod(GIT_STATUS);
    }

    public async Task<string> Pull()
    {
        return await CommonMethod(GIT_PULL_COMMAND);
    }

    public async Task<string> Push()
    {
        return await CommonMethod(GIT_PUSH_COMMAND);
    }

    #endregion
}
