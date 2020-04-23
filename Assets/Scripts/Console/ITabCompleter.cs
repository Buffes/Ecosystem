using System.Collections.Generic;

namespace Ecosystem.Console
{
    public interface ITabCompleter
    {
        List<string> OnTabComplete(string[] args);
    }
}
