namespace CLIWrapDemo.Example.GitClient;

public interface IGitClient
{
    Task<string> GetVersion();
    Task<string> Init();
    Task<string> Status();
    Task<string> Add();
    Task<string> Commit();
    Task<string> Pull();
    Task<string> Push();
    Task<string> Clone();
    Task<string> GetBranches();
    Task<string> ChangeBranche();
}
