using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// The age of infertility of an animal.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct InfertilityData : IComponentData
    {
        public float InfertilityAge;
    }
}