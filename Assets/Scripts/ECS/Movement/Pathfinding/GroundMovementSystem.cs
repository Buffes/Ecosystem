﻿using Ecosystem.ECS.Movement;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Ecosystem.ECS.Movement.Pathfinding
{

/// <summary>
/// Sets velocity and rotation based on movement input data.
/// </summary>
    public class GroundMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref Rotation rotation,
                ref PhysicsVelocity velocity,
                in MovementInput movementInput,
                in MovementSpeed movementSpeed) =>
            {

                float3 direction = movementInput.Direction;
                float speed = movementSpeed.Value;

                velocity.Linear.x = direction.x * speed;
                velocity.Linear.z = direction.z * speed;
                if (math.length(direction) != 0)
                {
                    rotation.Value = quaternion.LookRotation(direction, math.up());
                }

            }).ScheduleParallel();
        }
    }
}
