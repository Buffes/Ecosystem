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
    public class DeathEventSystem : ComponentSystem
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

            Entities.WithAll<DeathCommand>().ForEach((Entity entity, /*int entityInQueryIndex,*/
                ref DeathCommand deathCmd) =>
            {
                PostUpdateCommands.DestroyEntity(deathCmd.target);
                PostUpdateCommands.DestroyEntity(entity);

                //CommandBuffer or EntityManager??????

               /* commandBuffer.DestroyEntity(entityInQueryIndex, entity);
                commandBuffer.DestroyEntity(entityInQueryIndex, entity);*/

            });
        }
    }
}
