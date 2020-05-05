using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Represents the average lifespan of an animal. Stored in minutes.
    /// This value does not correspond exactly to when an animal will die.
    /// </summary>
    public struct LifespanData : IComponentData
    {
        public float Value;
    }
}