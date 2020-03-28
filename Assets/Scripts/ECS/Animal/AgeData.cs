using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// The age of an animal. Starts at 0.
    /// </summary>
    [Serializable]
    public struct AgeData : IComponentData
    {
        public float Age;
    }
}