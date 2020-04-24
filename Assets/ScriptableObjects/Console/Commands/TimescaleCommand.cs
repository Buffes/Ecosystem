using UnityEngine;

namespace Ecosystem.Console
{
    [CreateAssetMenu(menuName = "Console/Commands/TimescaleCommand")]
    public class TimescaleCommand : Command
    {
        public override void Execute(ICommandSender sender, string[] args)
        {
            if (!float.TryParse(args[0], out var value))
            {
                sender.SendMessage("Not a number: " + args[0], MessageType.Error);
                return;
            }

            Time.timeScale = value;
            sender.SendMessage("Timescale set to " + Time.timeScale);
        }
    }
}
