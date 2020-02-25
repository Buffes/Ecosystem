using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Targeting.Results
{
    [Serializable]
    public struct FoundPredator : IComponentData
    {
        public float3 Position;
    }
}
