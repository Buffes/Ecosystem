using System.Collections.Generic;
using System.Linq;

namespace Ecosystem.Console
{
    public class Console
    {
        private static readonly string UNKNOWN_COMMAND = "Unknown command";

        public string CommandPrefix { get; set; } = "/";
        public ICommandExecutor CommandExecutor { private get; set; }
        public ITabCompleter TabCompleter { private get; set; }

        private LinkedList<string> messageHistory = new LinkedList<string>();
        private LinkedListNode<string> currentHistoryNode;

        public Console() { }
        public Console(string commandPrefix) => this.CommandPrefix = commandPrefix;

        public void SendInput(ICommandSender sender, string input)
        {
            messageHistory.AddLast(input);

            if (IsCommand(input))
            {
                string[] inputSplit = SplitCommandInput(input);
                string label = inputSplit.FirstOrDefault();
                string[] args = inputSplit.Skip(1).ToArray();

                bool result = label != null &&
                    (CommandExecutor?.OnCommand(sender, label, args)).GetValueOrDefault();

                if (!result) sender.SendMessage(UNKNOWN_COMMAND, MessageType.Info);
                return;
            }

            sender.SendMessage(input, MessageType.Chat);
        }

        public List<string> GetAutocompleteAlternatives(string input)
        {
            if (!IsCommand(input)) return null;
            return TabCompleter.OnTabComplete(SplitCommandInput(input));
        }

        public void ResetHistoryNavigation() => currentHistoryNode = null;

        public string NavigatePreviousMessage()
        {
            if (currentHistoryNode == null) currentHistoryNode = messageHistory.Last;
            else if (currentHistoryNode.Previous != null) currentHistoryNode = currentHistoryNode.Previous;

            if (currentHistoryNode == null) return string.Empty;

            return currentHistoryNode.Value;
        }

        public string NavigateNextMessage()
        {
            if (currentHistoryNode != null) currentHistoryNode = currentHistoryNode.Next;
            if (currentHistoryNode == null) return string.Empty;

            return currentHistoryNode.Value;
        }

        private string[] SplitCommandInput(string input) => input.Remove(0, CommandPrefix.Length).Split(' ');

        private bool IsCommand(string input) => CommandPrefix != null && input.StartsWith(CommandPrefix);
    }
}
