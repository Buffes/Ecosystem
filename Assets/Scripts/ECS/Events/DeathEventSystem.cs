using System;
using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Ecosystem.ECS.Hybrid;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

namespace Ecosystem.ECS.Events
{
    /// <summary>
    /// Kills desired entities
    /// </summary>
    public class DeathEventSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
        private EntityManager entityManager;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            Entities.WithAll<DeathCommand>().ForEach((Entity entity, int entityInQueryIndex,
                in DeathCommand deathCommand) =>
            {
                commandBuffer.RemoveComponent<DeathCommand>(entityInQueryIndex, entity);
                commandBuffer.DestroyEntity(entityInQueryIndex, entity);
            }).ScheduleParallel();
        }
    }
}
