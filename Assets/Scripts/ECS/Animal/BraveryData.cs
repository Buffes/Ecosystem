using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Float value for calculating how often the animal will flee when noticing predators. Higher = braver
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct BraveryData : IComponentData
    {
        public float Value;
    }
}
