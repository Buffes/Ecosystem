using Unity.Entities;

namespace Ecosystem.ECS.Physics
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class PhysicsSystemGroup : ComponentSystemGroup
    {
    }
}