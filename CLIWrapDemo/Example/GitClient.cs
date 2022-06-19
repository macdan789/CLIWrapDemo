using CliWrap;
using CliWrap.Buffered;
using System.Text;

namespace CLIWrapDemo.Example;

public class GitClient : IGitClient
{
    #region CONSTANTS

    private const string GIT_COMMAND = "git";
    private const string GIT_COMMIT_COMMAND = "commit -m \"{0}\"";
    private const string GIT_ADD_COMMAND = "add *";
    private const string GIT_RESTORE_COMMAND = "restore --staged *";
    private const string GIT_ADD_SEPARATE_COMMAND = "add {0}";
    private const string GIT_INIT_COMMAND = "init";
    private const string GIT_PULL_COMMAND = "pull";
    private const string GIT_PUSH_COMMAND = "push";
    private const string GIT_CLONE_COMMAND = "clone";
    private const string GIT_BRANCH_COMMAND = "branch";
    private const string CURRENT = "--show-current";
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

    public async Task<string> Add(string file)
    {
        return await CommonMethod(string.Format(GIT_ADD_SEPARATE_COMMAND, file));
    }

    public async Task<string> Restore()
    {
        return await CommonMethod(GIT_RESTORE_COMMAND);
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
