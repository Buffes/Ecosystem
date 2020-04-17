

using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Moves entities with a LandCommand to the ground.
    /// </summary>
    public class LandingSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            Entities
            .ForEach((Entity entity, int entityInQueryIndex, 
                ref PhysicsVelocity velocity,
                in Translation translation,
                in MovementSpeed movementSpeed,
                in LandCommand landCommand) =>
            {
                float groundLevel = GetGroundLevel(translation.Value);
                float offset = 0.1f;
                if (translation.Value.y <= groundLevel + offset)
                {
                    commandBuffer.RemoveComponent<Flying>(entityInQueryIndex, entity);
                    commandBuffer.RemoveComponent<LandCommand>(entityInQueryIndex, entity);
                }
                else
                {
                    velocity.Linear.y = -movementSpeed.Value;
                }
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }

        private static float GetGroundLevel(float3 position)
        {
            return 0; // This can be replaced by a height map
        }
    }
}