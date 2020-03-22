using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Represents a kind of average lifespan of an animal. Stored in minutes.
    /// This value does not correspond exactly to when an animal will die,
    /// but to the point when the growth of the sigmoid function for probability of death is highest. 
    /// The animal will be very unlikely to die of old age much before this, and very likely much after this.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct LifespanData : IComponentData
    {
        public float Value;
    }
}