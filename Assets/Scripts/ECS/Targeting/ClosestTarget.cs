using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Targeting
{
    /// <summary>
    /// Marks the closest sensed Entity.
    /// </summary>
    public struct ClosestTarget : IComponentData
    {
        public Entity closestEntity; // The closest of the sensed targets.
        public float3 position;
    }
}
