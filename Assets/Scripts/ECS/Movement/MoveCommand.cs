using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Target position to find a path and move to.
    /// </summary>
    [Serializable]
    public struct MoveCommand : IComponentData
    {
        public float3 Target;
    }
}
