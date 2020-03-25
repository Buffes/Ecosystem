using Unity.Entities;
using Ecosystem.ECS.Animal;

namespace Ecosystem.ECS.Reproduction
{
    /// <summary>
    /// System for handling births. Affects entities with the BirthEvent component.
    /// </summary>
    public class BirthSystem : SystemBase
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
            Entities.WithAll<BirthEvent>().ForEach((Entity entity, int entityInQueryIndex
                , ref PregnancyData pregnancyData) =>
            {

            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
