using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Entities;

namespace Ecosystem.Console
{
    [CreateAssetMenu(menuName = "Console/Commands/ReloadSceneCommand")]
    public class ReloadSceneCommand : Command
    {
        public override void Execute(ICommandSender sender, string[] args)
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            entityManager.DestroyEntity(entityManager.UniversalQuery);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            sender.SendMessage("Scene reloaded");
        }
    }
}
