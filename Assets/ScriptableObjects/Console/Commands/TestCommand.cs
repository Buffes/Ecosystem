using UnityEngine;

namespace Ecosystem.Console
{
    [CreateAssetMenu(menuName = "Console/Commands/TestCommand")]
    public class TestCommand : Command
    {
        public override string Execute(string[] args)
        {
            return string.Concat(args).ToUpperInvariant();
        }
    }
}
