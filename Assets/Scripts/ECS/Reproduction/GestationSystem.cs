using Unity.Entities;
using Ecosystem.ECS.Animal;

namespace Ecosystem.ECS.Reproduction
{
    /// <summary>
    /// System for gestation 
    /// </summary>
    public class GestationSystem : SystemBase
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
            float deltaTime = Time.DeltaTime;

            Entities.ForEach((Entity entity, int entityInQueryIndex
                , ref GestationData gestationData) =>
            {
                gestationData.TimeSinceFertilisation += deltaTime / 1000.0f;
                if(gestationData.TimeSinceFertilisation >= gestationData.GestationPeriod)
                {
                    commandBuffer.AddComponent(entityInQueryIndex, entity, new BirthEvent());
                }
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
