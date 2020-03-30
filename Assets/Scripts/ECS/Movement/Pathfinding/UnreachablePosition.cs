using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Movement.Pathfinding
{
    /// <summary>
    /// Positions that are unreachable by the entity.
    /// </summary>
    [Serializable]
    [InternalBufferCapacity(20)]
    [GenerateAuthoringComponent]
    public struct UnreachablePosition : IBufferElementData
    {
        public int2 Position;
    }
}
