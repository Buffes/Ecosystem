using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Represents th expected lifespan of an animal. Stored in minutes.
    /// This value does not correspond exactly to when an animal will die,
    /// but to the point when it is most likely to die.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct LifespanData : IComponentData
    {
        public float Value;
    }
}