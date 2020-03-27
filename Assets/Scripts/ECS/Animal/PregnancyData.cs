using Unity.Entities;
using Ecosystem.Genetics;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Marks the animal as pregnant
    /// </summary>
    public class PregnancyData : IComponentData
    {
        public DNA DNAfromFather;
    }
}
