using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecosystem.Console
{
    public class HelpCommand : ICommand
    {
        private IEnumerable<ICommand> commands;

        public string Name { get; } = "help";
        public string[] Aliases { get; } = { "h", "commands" };
        public (int min, int max) ArgsRange { get; } = (0, 0);
        public string ArgsUsage { get; } = string.Empty;
        public string[] ArgsAlternatives { get; } = new string[0];

        public HelpCommand(IEnumerable<ICommand> commands)
        {
            this.commands = commands;
        }

        public void Execute(ICommandSender sender, string[] args)
        {
            sender.SendMessage(GetCommandList(commands));
        }

        private static string GetCommandList(IEnumerable<ICommand> commands)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Commands:");

            foreach (ICommand command in commands.Distinct())
            {
                sb.Append("\n    /").Append(command.Name).Append(" ").Append(command.ArgsUsage);
            }

            return sb.ToString();
        }
    }
}
