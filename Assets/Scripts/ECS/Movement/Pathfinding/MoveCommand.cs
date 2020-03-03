using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Movement.Pathfinding
{
    /// <summary>
    /// Move command expressing where an entity wants to go.
    /// </summary>
    [Serializable]
    public struct MoveCommand : IComponentData
    {
        public float3 target; // Target position to move to
        public float reach; // Distance away from the target to end the movement at
        public bool pathfind; // If to use pathfinding or to just move straight
        public float range; // Max range of a pathfinding solution
    }
}
