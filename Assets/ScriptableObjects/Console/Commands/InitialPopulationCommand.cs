using System.Linq;
using System.Text;
using UnityEngine;

namespace Ecosystem.Console
{
    [CreateAssetMenu(menuName = "Console/Commands/InitialPopulationCommand")]
    public class InitialPopulationCommand : Command
    {
        [SerializeField]
        private SimulationSettings simulationSettings = default;

        public override void Execute(ICommandSender sender, string[] args)
        {
            if (args.Length == 0 || (args.Length == 1 && args[0] == "list"))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Initial populations:");
                foreach (var pop in simulationSettings.InitialPopulations)
                {
                    sb.Append("\n    ").Append(pop.Prefab.name).Append(": ").Append(pop.Amount);
                }
                sender.SendMessage(sb.ToString());
                return;
            }

            if (args.Length == 1 && args[0] == "clear")
            {
                foreach (var pop in simulationSettings.InitialPopulations) pop.Amount = 0;
                sender.SendMessage("All initial populations set to 0");
                return;
            }

            if (args.Length != 2) return;
            var prefabName = args[0].ToLower();

            var population = simulationSettings.InitialPopulations.SingleOrDefault(x => x.Prefab.name == prefabName);
            
            if (population == null)
            {
                sender.SendMessage("Could not find \"" + prefabName + "\"");
                return;
            }

            if (!int.TryParse(args[1], out int amount))
            {
                sender.SendMessage("Not a number: " + args[1], MessageType.Error);
                return;
            }

            population.Amount = amount;
            sender.SendMessage("Initial " + population.Prefab.name + " population set to " + amount);
        }
    }
}
