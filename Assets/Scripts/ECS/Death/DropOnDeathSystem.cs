﻿using Unity.Entities;
using Unity.Transforms;

namespace Ecosystem.ECS.Death
{
    /// <summary>
    /// Drops items on death.
    /// </summary>
    public class DropOnDeathSystem : SystemBase
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
                .WithAll<DeathEvent>()
                .ForEach((int entityInQueryIndex, in DropOnDeath dropOnDeath, in Translation position) =>
            {

                Entity drop = commandBuffer.Instantiate(entityInQueryIndex, dropOnDeath.Prefab);
                var value = position.Value;
                value.y -= 0.390f;
                commandBuffer.SetComponent(entityInQueryIndex, drop, new Translation { Value = value });

            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
