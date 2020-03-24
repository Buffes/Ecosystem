using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct GestationData : IComponentData
    {
        public float GestationPeriod;
    }
}
