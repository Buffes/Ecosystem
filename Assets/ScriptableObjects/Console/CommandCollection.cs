using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Console
{
    [CreateAssetMenu(menuName = "Console/CommandCollection")]
    public class CommandCollection : ScriptableObject
    {
        [SerializeField]
        private List<Command> commands = new List<Command>();

        public List<Command> Commands => commands;
    }
}
