namespace Ecosystem.Console
{
    public interface ICommandExecutor
    {
        bool OnCommand(ICommandSender sender, string label, string[] args);
    }
}
