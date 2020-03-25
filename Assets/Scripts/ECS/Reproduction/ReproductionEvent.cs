using Unity.Entities;

namespace Ecosystem.ECS.Reproduction
{
    /// <summary>
    /// Marks the entity to reproduce. Should only be attached to the animal during the act of reproduction (both animals that is).
    /// </summary>
    public struct ReproductionEvent : IComponentData
    {
        public Entity Partner;
    }
}
