using UnityEngine;

namespace Ecosystem.Console
{
    [CreateAssetMenu(menuName = "Console/Commands/PauseCommand")]
    public class PauseCommand : Command
    {
        public override void Execute(ICommandSender sender, string[] args)
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            sender.SendMessage(Time.timeScale == 0 ? "Paused" : "Unpaused");
        }
    }
}
