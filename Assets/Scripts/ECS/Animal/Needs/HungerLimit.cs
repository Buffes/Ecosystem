using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Needs {
    /// <summary>
    /// A float value ranging from 0 to infinity indicating the limit of when to search for food
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct HungerLimit : IComponentData
    {
        public float Value;
    }
}
