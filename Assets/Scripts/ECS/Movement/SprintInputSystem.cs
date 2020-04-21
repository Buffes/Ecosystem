using Ecosystem.ECS.Animal;
using Unity.Entities;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Makes entities (trying to sprint and not exhausted) sprint.
    /// </summary>
    public class SprintInputSystem : SystemBase
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
                .WithAll<SprintInput>()
                .WithNone<Sprinting, ExhaustedData, Death.DeathEvent>()
                .ForEach((Entity entity, int entityInQueryIndex) =>
                {
                    commandBuffer.AddComponent<Sprinting>(entityInQueryIndex, entity);
                }).ScheduleParallel();

            Entities
                .WithAll<Sprinting>()
                .WithNone<SprintInput>()
                .ForEach((Entity entity, int entityInQueryIndex) =>
                {
                    commandBuffer.RemoveComponent<Sprinting>(entityInQueryIndex, entity);
                }).ScheduleParallel();

            Entities
                .WithAll<Sprinting, ExhaustedData>()
                .ForEach((Entity entity, int entityInQueryIndex) =>
                {
                    commandBuffer.RemoveComponent<Sprinting>(entityInQueryIndex, entity);
                }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
