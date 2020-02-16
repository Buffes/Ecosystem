using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Physics
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct Velocity : IComponentData
    {
        public float3 Value;
    }
}
