

using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

using Ecosystem.ECS.Events;

namespace Ecosystem.ECS.Animal.Needs
{
    /// <summary>
    /// System for increasing hunger on an animal.
    /// </summary>
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
                hungerData.Hunger += Time.DeltaTime / 1000.0f;

                if(hungerData.Hunger <= 0.0f)
                {
                    commandBuffer.AddComponent<DeathEvent>(entityInQueryIndex, entity);
                }

            }).ScheduleParallel();
            throw new NotImplementedException();
        }
    }
}
