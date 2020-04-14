using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Defines which types of terrain the entity can move on.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct MovementTerrain : IComponentData
    {
        public bool MovesOnLand;
        public bool MovesOnWater;
    }
}
