﻿using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Physics
{
    /// <summary>
    /// Stops everything from falling through the ground.
    /// </summary>
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    public class GroundCollisionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Translation translation, ref Velocity velocity) =>
            {

                float groundLevel = GetGroundLevel(translation.Value);

                if (translation.Value.y < groundLevel)
                {
                    translation.Value.y = groundLevel;
                    velocity.Value.y = 0;
                }

            }).ScheduleParallel();
        }

        private static float GetGroundLevel(float3 position)
        {
            return 0; // This can be replaced by a height map
        }
    }
}
