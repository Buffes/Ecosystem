using Unity.Entities;
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
                , ReproductionEvent reproductionEvent
                , in SexData sexData) =>
            {
                if(sexData.Sex == Sex.Female)
                {
                    EntityManager.AddComponentData(entity, new PregnancyData { DNAfromFather = reproductionEvent.PartnerDNA }); // If female, become pregnant
                }
                EntityManager.RemoveComponent<ReproductionEvent>(entity);

            }).WithoutBurst().Run();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
