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
        public float3 Target; // Target position to move to
        public float Reach; // Distance away from the target to end the movement at
        public bool Pathfind; // If to use pathfinding or to just move straight
        public int MaxTiles; // Max number of tiles to search before stopping
    }
}
