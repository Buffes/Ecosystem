using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Ecosystem.ECS.Physics
{
    /// <summary>
    /// Applies gravity to all entities with a Velocity component.
    /// </summary>
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateBefore(typeof(GroundCollisionSystem))]
    public class GravitySystem : JobComponentSystem
    {
        private static readonly float3 GRAVITY = new float3(0, -9.8f, 0);

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            float deltaTime = Time.DeltaTime;

            return Entities.ForEach((ref Velocity velocity) =>
            {

                velocity.Value += math.up() * GRAVITY * deltaTime;

            }).Schedule(inputDependencies);
        }
    }
}
