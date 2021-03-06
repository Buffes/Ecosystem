﻿using Unity.Entities;
using Ecosystem.ECS.Death;

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

            float deltaTime = Time.DeltaTime/60f;

            Entities
                .WithNone<DeathEvent>()
                .ForEach((Entity entity, int entityInQueryIndex,
                ref ThirstData thirstData) =>
                {
                    thirstData.Thirst -= deltaTime;

                    if(thirstData.Thirst <= 0.0f)
                    {
                        commandBuffer.AddComponent<DeathEvent>(entityInQueryIndex, entity,new DeathEvent(DeathCause.Thirst));
                    }

                }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
