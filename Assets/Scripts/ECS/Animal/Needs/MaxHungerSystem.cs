using Unity.Entities;
using Ecosystem.ECS.Death;

namespace Ecosystem.ECS.Animal.Needs
{
    /// <summary>
    /// System for clamping down hunger on an animal.
    /// </summary>
    public class MaxHungerSystem : SystemBase
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

            Entities.ForEach((Entity entity,int entityInQueryIndex,MaxHungerData maxHungerData,
                ref HungerData hungerData) =>
            {
                    if (hungerData.Hunger >= maxHungerData.MaxHunger)
                    {
                        hungerData.Hunger = maxHungerData.MaxHunger;
                    }

                }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
