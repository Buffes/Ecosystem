using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Defines which types of terrain the entity can move on.
    /// </summary>
    public struct MovementTerrain : IComponentData
    {
        public bool MovesOnLand;
        public bool MovesOnWater;
    }
}
