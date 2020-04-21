using System.Collections.Generic;

namespace Ecosystem.Console
{
    public class CommandManager : ICommandExecutor
    {
        private readonly Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        public CommandManager() { }
        public CommandManager(IEnumerable<ICommand> commands) => Register(commands);

        public string OnCommand(string label, string[] args)
        {
            if (!commands.TryGetValue(label, out ICommand command)) return null;

            return command.Execute(args);
        }

        public void Register(IEnumerable<ICommand> commands)
        {
            foreach (ICommand command in commands) Register(command);
        }

        public void Register(ICommand command)
        {
            Register(command.Name, command);

            foreach (string alias in command.Aliases)
            {
                Register(alias, command);
            }
        }

        private void Register(string key, ICommand command)
        {
            if (string.IsNullOrEmpty(key)) return;
            commands[key] = command;
        }
    }
}
