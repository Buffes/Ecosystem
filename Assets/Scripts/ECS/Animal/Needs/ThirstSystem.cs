using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Needs
{
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
            Entities.ForEach((Entity entity, int entityInQueryIndex,
                ref ThirstData thirstData) =>
            {

            });

            throw new NotImplementedException();
        }
    }
}
