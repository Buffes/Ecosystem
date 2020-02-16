using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Ecosystem.ECS.Physics
{
    /// <summary>
    /// Moves all entities based on their velocity.
    /// </summary>
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateBefore(typeof(GravitySystem))]
    public class VelocitySystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            float deltaTime = Time.DeltaTime;

            return Entities.ForEach((
                ref Translation translation,
                in Velocity velocity) =>
            {

                translation.Value += velocity.Value * deltaTime;

            }).Schedule(inputDependencies);
        }
    }
}
