using Unity.Entities;
using Ecosystem.ECS.Animal;
using Ecosystem.Genetics;

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
            var entityManager = m_EndSimulationEcbSystem.EntityManager;

            Entities.WithAll<BirthEvent>().ForEach((Entity entity, int entityInQueryIndex
                , ref PregnancyData pregnancyData) =>
            {
                //TODO Spawn new animal of the same type as parents

                DNA newDNA = DNA.InheritedDNA(EntityManager.GetComponentData<DNA>(entity), pregnancyData.DNAfromFather); // inherit genes from parents
                
                
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
