using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Needs {
    /// <summary>
    /// A float value ranging from 0 to infinity indicating the limit of when to search for water
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct ThirstLimit : IComponentData
    {
        public float Value;
    }
}
