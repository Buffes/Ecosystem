using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

/// <summary>
/// Stops everything from falling through the ground.
/// 
/// This system can use a height map to represent the ground level in the world or just use a
/// flat number for a flat world. Even a flat world could, for example, want to support lower
/// ground levels in water to allow fish to swim underneath the surface.
/// 
/// The system could also be replaced by adding a regular ground plane
/// in the editor.
/// </summary>
[UpdateInGroup(typeof(PhysicsSystemGroup))]
public class GroundCollisionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation translation, ref PhysicsVelocity velocity, ref PhysicsMass mass) =>
        {
            float groundLevel = GetGroundLevel(translation.Value);

            if (translation.Value.y < groundLevel)
            {
                translation.Value.y = groundLevel;
                velocity.Linear.y = 0;
            }

        }).ScheduleParallel();
    }

    private static float GetGroundLevel(float3 position)
    {
        return 0; // This can be replaced by a height map
    }
}
