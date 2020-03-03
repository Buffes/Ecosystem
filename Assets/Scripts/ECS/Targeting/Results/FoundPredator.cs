using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Targeting.Results
{
    [Serializable]
    public struct FoundPredator : IComponentData
    {
        public Entity Entity;
        public float3 Position;
    }
}
