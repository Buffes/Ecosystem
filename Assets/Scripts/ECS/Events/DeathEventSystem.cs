using Unity.Entities;

namespace Ecosystem.ECS.Events
{
    /// <summary>
    /// Kills desired entities
    /// </summary>
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

            Entities.ForEach((Entity entity, int entityInQueryIndex,
                in DeathEvent deathCmd) =>
            {
                commandBuffer.DestroyEntity(entityInQueryIndex, deathCmd.Target);
                commandBuffer.DestroyEntity(entityInQueryIndex, entity);

            }).ScheduleParallel();
        }
    }
}
