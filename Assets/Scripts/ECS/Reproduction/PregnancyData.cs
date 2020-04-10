using Unity.Entities;
using Ecosystem.Genetics;

namespace Ecosystem.ECS.Reproduction
{
    /// <summary>
    /// Carries the DNA of the baby.
    /// </summary>
    public class PregnancyData : IComponentData
    {
        public DNA DNAforBaby;
    }
}
