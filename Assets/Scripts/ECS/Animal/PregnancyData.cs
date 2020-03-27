using Unity.Entities;
using Ecosystem.Genetics;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Marks the animal as pregnant
    /// </summary>
    public struct PregnancyData : IComponentData
    {
        public DNA DNAfromFather;
    }
}
