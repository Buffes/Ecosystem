using Unity.Entities;

namespace Ecosystem.ECS.Movement
{

    /// <summary>
    /// Trying to sprint.
    /// <para/>
    /// Sometimes the entity wants to sprint but can't (e.g., being exhausted).
    /// </summary>
    public struct SprintInput : IComponentData
    {
    }
}
