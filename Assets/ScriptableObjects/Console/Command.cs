using UnityEngine;

namespace Ecosystem.Console
{
    public abstract class Command : ScriptableObject, ICommand
    {
        [SerializeField]
        private new string name = string.Empty;
        [SerializeField]
        private string[] aliases = new string[0];

        public string Name => name;

        public string[] Aliases => aliases;

        public abstract string Execute(string[] args);
    }
}
