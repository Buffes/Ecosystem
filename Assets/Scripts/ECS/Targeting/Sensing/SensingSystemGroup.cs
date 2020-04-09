using Ecosystem.ECS.Grid.Buckets;
using Unity.Entities;

namespace Ecosystem.ECS.Targeting.Sensing
{
    /// <summary>
    /// Sensing systems order:
    /// 1. Clear sensed entities buffers
    /// 2. Add sensed entities (various sources)
    /// </summary>
    public class SensingSystemGroup : ComponentSystemGroup { }

    [UpdateInGroup(typeof(SensingSystemGroup))]
    public class SensingResetSystemGroup : ComponentSystemGroup { }

    [UpdateInGroup(typeof(SensingSystemGroup))]
    [UpdateAfter(typeof(SensingResetSystemGroup))]
    public class SensingAddSystemGroup : ComponentSystemGroup { }
}
