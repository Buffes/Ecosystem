using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Movement.Pathfinding
{
    /// <summary>
    /// Positions that are unreachable by the entity. Timestamp for removing outdated positions
    /// </summary>
    [Serializable]
    [InternalBufferCapacity(20)]
    public struct UnreachablePosition : IBufferElementData
    {
        public int2 Position;
        public double Timestamp;
    }
}
