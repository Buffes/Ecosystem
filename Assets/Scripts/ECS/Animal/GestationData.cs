using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Time for the babies to birth
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct GestationData : IComponentData
    {
        public float GestationPeriod; // Max Value
        public float TimeSinceFertilisation;
    }
}
