using Ecosystem.ECS.Movement;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;


namespace Ecosystem.ECS.Physics
{

    /// <summary>
    /// Sets the gravity factor to zero for flying objects, and to 1 for non-flying objects.
    /// </summary>
    public class ToggleGravitySystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            Entities
                .WithAll<Flying>()
                .WithNone<PhysicsGravityFactor>()
                .ForEach((Entity entity, int entityInQueryIndex) =>
            {

                commandBuffer.AddComponent<PhysicsGravityFactor>(entityInQueryIndex, entity, new PhysicsGravityFactor {Value = 0f});

            }).ScheduleParallel();

            Entities
                .WithNone<Flying>()
                .ForEach((Entity entity, int entityInQueryIndex, ref PhysicsGravityFactor gravityFactor) =>
            {
                gravityFactor.Value = 1f;
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
