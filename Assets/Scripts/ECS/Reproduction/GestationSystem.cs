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
            float deltaTime = Time.DeltaTime / 60f;


            Entities.ForEach((Entity entity, int entityInQueryIndex,
                ref Pregnant pregnant) =>
            {
                pregnant.RemainingDuration -= deltaTime;
                if(pregnant.RemainingDuration <= 0)
                {
                    commandBuffer.AddComponent(entityInQueryIndex, entity, new BirthEvent());
                    commandBuffer.RemoveComponent<Pregnant>(entityInQueryIndex, entity);
                }
            }).Run();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
