using CliWrap;
using CliWrap.Buffered;

namespace CLIWrapDemo.Example;

public class GitClient : IGitClient
{
    #region CONSTANTS

    private const string GIT_COMMAND = "git";
    private const string GIT_COMMIT_COMMAND = "commit";
    private const string GIT_ADD_COMMAND = "add *";
    private const string GIT_ADD_SEPARATE_COMMAND = "add {0}";
    private const string GIT_INIT_COMMAND = "init";
    private const string GIT_PULL_COMMAND = "pull";
    private const string GIT_PUSH_COMMAND = "push";
    private const string GIT_CLONE_COMMAND = "clone";
    private const string GIT_BRANCH_COMMAND = "branch";
    private const string GIT_CHECKOUT_COMMAND = "checkout";
    private const string GIT_VERSION_COMMAND = "--version";
    private readonly string workingPath;

    #endregion


    public GitClient(string path)
    {
        workingPath = path;
    }

    private async Task<string> CommonMethod(params string[] args)
    {
        try
        {
            var result = await Cli.Wrap(GIT_COMMAND)
                .WithArguments(args)
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
    
    public async Task<string> Add()
    {
        return await CommonMethod(GIT_ADD_COMMAND);
    }

    public async Task<string> Add(params string[] files)
    {
        return await CommonMethod(string.Format(GIT_ADD_SEPARATE_COMMAND, string.Join(" ", files)));
    }

    public Task<string> ChangeBranche(string branch)
    {
        throw new NotImplementedException();
    }

    public Task<string> Clone(string source)
    {
        throw new NotImplementedException();
    }

    public Task<string> Commit(string message)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetBranches()
    {
        return await CommonMethod(GIT_BRANCH_COMMAND);
    }

    public Task<string> GetCurrentBranch()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetVersion()
    {
        throw new NotImplementedException();
    }

    public Task<string> Init()
    {
        throw new NotImplementedException();
    }

    public Task<string> Pull()
    {
        throw new NotImplementedException();
    }

    public Task<string> Push()
    {
        throw new NotImplementedException();
    }
}
