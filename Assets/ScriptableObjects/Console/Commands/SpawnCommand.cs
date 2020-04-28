using Ecosystem.ECS.Grid;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.Console
{
    [CreateAssetMenu(menuName = "Console/Commands/SpawnCommand")]
    public class SpawnCommand : Command
    {
        [SerializeField]
        private SimulationSettings simulationSettings = default;

        private WorldGridSystem worldGridSystem;

        public override void Execute(ICommandSender sender, string[] args)
        {
            var prefabName = args[0].ToLower();

            if (!int.TryParse(args[1], out int amount))
            {
                sender.SendMessage("Not a number: " + args[1], MessageType.Error);
                return;
            }

            var population = simulationSettings.InitialPopulations.SingleOrDefault(x => x.Prefab.name == prefabName);

            if (population == null)
            {
                sender.SendMessage("Could not find \"" + prefabName + "\"");
                return;
            }

            var prefab = population.Prefab;

            if (worldGridSystem == null) worldGridSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<WorldGridSystem>();

            PrefabSpawner.Spawn(prefab, amount, worldGridSystem);
            sender.SendMessage("Spawned " + amount + " " + prefab.name + "s");
        }
    }
}
