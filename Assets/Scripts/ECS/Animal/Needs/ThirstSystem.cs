using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

using Ecosystem.ECS.Events;

namespace Ecosystem.ECS.Animal.Needs
{
    /// <summary>
    /// System for increasing the thirst of an animal.
    /// </summary>
    public class ThirstSystem : SystemBase
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
                ref ThirstData thirstData) =>
            {
                thirstData.Thirst += Time.DeltaTime / 1000.0f;

                if(thirstData.Thirst > 1.0f)
                {
                    commandBuffer.AddComponent<DeathEvent>(entityInQueryIndex, entity);
                }

            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
