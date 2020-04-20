using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Targeting.Targets
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct LookingForFleeTarget : IComponentData
    {
        public bool HasFound;
        public float3 Position;
        public float3 EnemyPosition;
    }
}
