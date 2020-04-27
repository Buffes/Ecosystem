using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Needs {
    /// <summary>
    /// A float value ranging from 0 to infinity indicating the limit of when to search for mate
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct MatingLimit : IComponentData
    {
        public float Value;
    }
}
