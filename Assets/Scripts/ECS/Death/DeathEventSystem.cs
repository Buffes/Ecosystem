using Unity.Entities;

namespace Ecosystem.ECS.Death
{
    /// <summary>
    /// Kills desired entities
    /// </summary>
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class DeathEventSystem : SystemBase
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

            Entities.WithAll<DeathEvent>().ForEach((Entity entity, int entityInQueryIndex) =>
            {
                commandBuffer.DestroyEntity(entityInQueryIndex, entity);
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
