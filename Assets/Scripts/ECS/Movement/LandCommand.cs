using Unity.Entities;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Command for the entity to stop flying and move down to land.
    /// </summary>
    public struct LandCommand : IComponentData
    {
    }
}
