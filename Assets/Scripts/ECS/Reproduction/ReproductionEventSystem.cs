﻿using Unity.Entities;
using Ecosystem.ECS.Animal;
using Ecosystem.Genetics;

namespace Ecosystem.ECS.Reproduction
{
    /// <summary>
    /// System for animals to become pregnant during the reproduction event. The idea is for the event to occur for both the animals seperatly.
    /// </summary>
    public class ReproductionEventSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();
            Entities.ForEach((Entity entity, int entityInQueryIndex
                ,ref ReproductionEvent reproductionEvent
                ,in SexData sexData) =>
            {
                if(sexData.Sex == Sex.Female)
                {
                    commandBuffer.AddComponent(entityInQueryIndex, entity, new PregnancyData() { DNAfromFather = EntityManager.GetComponentData<DNA>(reproductionEvent.Partner) }); // If female, become pregnant
                }
                commandBuffer.RemoveComponent<ReproductionEvent>(entityInQueryIndex, entity);

            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
