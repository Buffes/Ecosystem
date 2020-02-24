using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Targeting.Results
{
    [Serializable]
    public struct FoundFood : IComponentData
    {
        public float3 Position;
    }
}
