using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Animal.Needs
{
    public class HungerSystem : SystemBase
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

            Entities.ForEach((Entity entity, int entityInQueryIndex,
                ref HungerData hungerData) =>
            {
                hungerData.Hunger -= Time.DeltaTime / 100.0f;

                if(hungerData.Hunger <= 0.0f)
                {
                    //Die
                }

            }).ScheduleParallel();
            throw new NotImplementedException();
        }
    }
}
