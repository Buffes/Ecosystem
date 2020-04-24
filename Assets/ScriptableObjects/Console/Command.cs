using MinMaxSlider;
using UnityEngine;

namespace Ecosystem.Console
{
    public abstract class Command : ScriptableObject, ICommand
    {
        [SerializeField] private new string name = string.Empty;
        [SerializeField] private string[] aliases = new string[0];
        [Header("Arguments")]
        [MinMaxSlider(0, 5)]
        [SerializeField] private Vector2Int argsRange = default;
        [SerializeField] private string argsUsage = string.Empty;
        [SerializeField] private string[] argsAlternatives = new string[0];

        public string Name => name;
        public string[] Aliases => aliases;
        public (int min, int max) ArgsRange => (argsRange.x, argsRange.y);
        public string ArgsUsage => argsUsage;
        public string[] ArgsAlternatives => argsAlternatives;

        public abstract void Execute(ICommandSender sender, string[] args);
    }
}
