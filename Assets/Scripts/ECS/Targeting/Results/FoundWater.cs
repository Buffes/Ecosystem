using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Targeting.Results
{
    [Serializable]
    public struct FoundWater : IComponentData
    {
        public float3 Position;
    }
}
