using Ecosystem.ECS.Movement;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

/// <summary>
/// Sets velocity and rotation based on movement input data.
/// </summary>
public class GroundMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        Entities.ForEach((
            ref Rotation rotation,
            ref Translation position,
            in MovementInput movementInput,
            in MovementStats movementStats) =>
        {

            float3 direction = movementInput.Direction;
            float speed = movementInput.Sprint ?
                movementStats.SprintSpeed : movementStats.Speed;

            //velocity.Linear.x = direction.x * speed;
            //velocity.Linear.z = direction.z * speed;
            if (math.length(direction) != 0)
            {
                rotation.Value = quaternion.LookRotation(direction, math.up());
            }
            position.Value += math.forward(rotation.Value) * 6f * deltaTime;

        }).ScheduleParallel();
    }
}
