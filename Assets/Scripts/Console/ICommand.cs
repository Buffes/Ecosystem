namespace Ecosystem.Console
{
    public interface ICommand
    {
        string Name { get; }
        string[] Aliases { get; }
        (int min, int max) ArgsRange { get; }
        string ArgsUsage { get; }
        string[] ArgsAlternatives { get; }

        void Execute(ICommandSender sender, string[] args);
    }
}
