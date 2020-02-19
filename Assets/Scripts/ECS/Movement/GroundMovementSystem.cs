using Ecosystem.ECS.Physics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Movement
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
                ref Velocity velocity,
                in MovementInput movementInput,
                in MovementStats movementStats) =>
            {

                float3 direction = movementInput.Direction;
                float speed = movementInput.Sprint ?
                    movementStats.SprintSpeed : movementStats.Speed;

                velocity.Value.x = direction.x * speed;
                velocity.Value.z = direction.z * speed;
                if (math.length(direction) != 0)
                {
                    rotation.Value = quaternion.LookRotation(direction, math.up());
                }

            }).ScheduleParallel();
        }
    }
}
