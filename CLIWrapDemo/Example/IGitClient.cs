namespace CLIWrapDemo.Example;

public interface IGitClient
{
    Task<string> GetVersion();
    Task<string> Init();
    Task<string> Add();
    Task<string> Add(string file);
    Task<string> Commit(string message);
    Task<string> Pull();
    Task<string> Push();
    Task<string> Clone(string source);
    Task<string> GetCurrentBranch();
    Task<string> GetBranches();
    Task<string> ChangeBranche(string branch);

}
