using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Movement.Pathfinding
{
    /// <summary>
    /// Checkpoint positions forming a path to reach a target.
    /// </summary>
    [Serializable]
    [InternalBufferCapacity(20)]
    public struct PathElement : IBufferElementData
    {
        public float3 Checkpoint;
    }
}
