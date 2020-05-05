using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecosystem.Console
{
    public class CommandManager : ITabExecutor
    {
        private static readonly string INVALID_INPUT = "Invalid input";

        private readonly Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        public CommandManager() => Register(new HelpCommand(commands.Values));
        public CommandManager(IEnumerable<ICommand> commands) : this() => Register(commands);

        public bool OnCommand(ICommandSender sender, string label, string[] args)
        {
            if (!commands.TryGetValue(label, out ICommand command)) return false;
            if (!ValidateInput(command, args))
            {
                sender.SendMessage(INVALID_INPUT, MessageType.Error);
                sender.SendMessage("Correct usage: /" + command.Name + " " + command.ArgsUsage, MessageType.Info);
                return true;
            }

            command.Execute(sender, args);
            return true;
        }

        public List<string> OnTabComplete(string[] args)
        {
            List<string> result = new List<string>();

            if (args.Length == 0) return result;
            string commandInput = args[0];
            if (args.Length == 1)
            {
                foreach (ICommand command in commands.Values)
                {
                    if (command.Name.StartsWith(commandInput)) result.Add(command.Name);
                    foreach (string alias in command.Aliases)
                    {
                        if (alias.StartsWith(commandInput)) result.Add(alias);
                    }
                }
            }
            else if (commands.TryGetValue(commandInput, out ICommand command))
            {
                string[] argsInput = args.Skip(1).ToArray();

                foreach (string argsAlternative in command.ArgsAlternatives)
                {
                    string[] argsAlternativeArray = argsAlternative.Split(' ');
                    if (argsInput.Length > argsAlternativeArray.Length) continue;
                    int currentArgIndex = argsInput.Length - 1;
                    bool matching = true;
                    for (int i = 0; i < currentArgIndex; i++)
                    {
                        if (!argsInput[i].Equals(argsAlternativeArray[i]))
                        {
                            matching = false;
                            break;
                        }
                    }
                    if (!matching) continue;

                    string currentAltArg = argsAlternativeArray[currentArgIndex];
                    if (currentAltArg.StartsWith(argsInput[currentArgIndex], StringComparison.OrdinalIgnoreCase))
                    {
                        result.Add(currentAltArg);
                    }
                }
            }

            return result;
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

        private bool ValidateInput(ICommand command, string[] args)
        {
            if (args.Length < command.ArgsRange.min) return false;
            if (args.Length > command.ArgsRange.max && command.ArgsRange.max >= 0) return false;
            return true;
        }
    }
}
