using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Targeting.Targets
{
    [Serializable]
    public struct LookingForWater : IComponentData
    {
        public bool HasFound;
        public Entity Entity;
        public float3 Position;
    }
}
