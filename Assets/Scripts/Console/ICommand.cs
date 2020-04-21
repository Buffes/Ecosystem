namespace Ecosystem.Console
{
    public interface ICommand
    {
        string Name { get; }
        string[] Aliases { get; }
        string Execute(string[] args);
    }
}
