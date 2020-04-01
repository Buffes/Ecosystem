using Unity.Entities;
using Ecosystem.Genetics;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Marks the animal as pregnant. Carries the DNA of the baby and the time since fertilisation.
    /// </summary>
    public class PregnancyData : IComponentData
    {
        public DNA DNAforBaby;
        public float TimeSinceFertilisation;
    }
}
