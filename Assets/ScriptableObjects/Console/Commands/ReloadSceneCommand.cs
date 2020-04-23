using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ecosystem.Console
{
    [CreateAssetMenu(menuName = "Console/Commands/ReloadSceneCommand")]
    public class ReloadSceneCommand : Command
    {
        public override void Execute(ICommandSender sender, string[] args)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            sender.SendMessage("Scene reloaded");
        }
    }
}
