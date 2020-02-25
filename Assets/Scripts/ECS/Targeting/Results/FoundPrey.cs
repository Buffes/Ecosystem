using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Targeting.Results
{
    [Serializable]
    public struct FoundPrey : IComponentData
    {
        public float3 Position;
    }
}
