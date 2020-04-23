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
            sender.SendMessage("Reloading scene...");
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            DestroyAllEntities();
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private static void DestroyAllEntities()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            entityManager.DestroyEntity(entityManager.UniversalQuery);
        }
    }
}
