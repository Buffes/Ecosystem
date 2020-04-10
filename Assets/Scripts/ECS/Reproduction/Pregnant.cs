using Unity.Entities;

namespace Ecosystem.ECS.Reproduction
{
    /// <summary>
    /// Marks the entity as pregnant.
    /// </summary>
    public struct Pregnant : IComponentData
    {
        public float RemainingDuration; // Time left in seconds before giving birth
    }
}
