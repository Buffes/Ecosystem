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

            Entities
                .WithoutBurst()
                .ForEach
                ((Entity entity , int entityInQueryIndex
                , PregnancyData pregnancyData
                , in GestationData gestationData) =>
            {
                pregnancyData.TimeSinceFertilisation += deltaTime / 1000.0f;
                if(pregnancyData.TimeSinceFertilisation >= gestationData.GestationPeriod)
                {
                    
                    //EntityManager.AddComponentData(entity, new BirthEvent());
                    commandBuffer.AddComponent(entityInQueryIndex, entity, new BirthEvent());
                }
            }).Run();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
