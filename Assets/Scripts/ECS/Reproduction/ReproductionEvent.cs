using Unity.Entities;
using Ecosystem.Genetics;

namespace Ecosystem.ECS.Reproduction
{
    /// <summary>
    /// Marks the entity to reproduce. Should only be attached to the animal during the act of reproduction (both animals that is).
    /// </summary>
    public class ReproductionEvent : IComponentData
    {
        public DNA PartnerDNA;
    }
}
