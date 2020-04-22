using System.Linq;

namespace Ecosystem.Console
{
    public class Console
    {
        private static readonly string INVALID_COMMAND = "Unknown command";

        public string CommandPrefix { get; set; } = "/";
        public ICommandExecutor CommandExecutor { private get; set; }

        public Console() { }
        public Console(string commandPrefix) => this.CommandPrefix = commandPrefix;

        public void SendInput(ICommandSender sender, string input)
        {
            if (IsCommand(input))
            {
                string[] inputSplit = input.Remove(0, CommandPrefix.Length).Split(' ');
                string label = inputSplit.FirstOrDefault();
                string[] args = inputSplit.Skip(1).ToArray();

                string returnMessage = label == null ? null : CommandExecutor?.OnCommand(label, args);
                sender.SendMessage(returnMessage != null ? returnMessage : INVALID_COMMAND);
                return;
            }

            sender.SendMessage(input);
        }

        private bool IsCommand(string input) => CommandPrefix != null && input.StartsWith(CommandPrefix);
    }
}
