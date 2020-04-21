namespace Ecosystem.Console
{
    public interface ICommandExecutor
    {
        string OnCommand(string label, string[] args);
    }
}
