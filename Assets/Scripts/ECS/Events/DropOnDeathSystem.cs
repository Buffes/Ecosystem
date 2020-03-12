using Ecosystem.ECS.Animal;
using Unity.Entities;
using Unity.Transforms;

namespace Ecosystem.ECS.Events
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
                commandBuffer.SetComponent(entityInQueryIndex, drop, new Translation { Value = position.Value });

            }).ScheduleParallel();
        }
    }
}
