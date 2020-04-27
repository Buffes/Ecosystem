using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Float value for calculating how often the animal will flee when noticing predators.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct FearData : IComponentData
    {
        public float Value;
    }
}
