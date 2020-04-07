using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Targeting.Targets
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct LookingForRandomTarget : IComponentData
    {
        public bool HasFound;
        public float3 Position;
    }
}
