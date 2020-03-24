using Unity.Entities;

namespace Ecosystem.ECS.Stats
{
    /// <summary>
    /// Stat systems order:
    /// 1. Reset to base values
    /// 2. Apply percent modifiers
    /// </summary>
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class StatSystemGroup : ComponentSystemGroup { }

    [UpdateInGroup(typeof(StatSystemGroup))]
    public class BaseStatSystemGroup : ComponentSystemGroup { }

    [UpdateInGroup(typeof(StatSystemGroup))]
    [UpdateAfter(typeof(BaseStatSystemGroup))]
    public class StatModifierSystemGroup : ComponentSystemGroup { }

    [UpdateInGroup(typeof(StatModifierSystemGroup))]
    public class PercentStatModifierSystemGroup : ComponentSystemGroup { }
}
