using Ecosystem.ECS.Targeting.Sensors;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Entities;
using Unity.Transforms;

namespace Ecosystem.ECS.Targeting.FindSystems
{
    public class FindSystem : SystemBase
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
                .WithNone<DynamicBuffer<DetectedEntityElement>>()
                .ForEach((Entity entity, int entityInQueryIndex) =>
            {
                DynamicBuffer<DetectedEntityElement> detectedEntities = commandBuffer.AddBuffer<DetectedEntityElement>(entityInQueryIndex,entity);
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
