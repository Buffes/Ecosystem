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
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .ForEach((Entity entity
                , ReproductionEvent reproductionEvent
                , in SexData sexData) =>
            {
                if(sexData.Sex == Sex.Female)
                {
                    EntityManager.AddComponentData(entity, new PregnancyData { DNAfromFather = reproductionEvent.PartnerDNA }); // If female, become pregnant
                }
                EntityManager.RemoveComponent<ReproductionEvent>(entity);

            }).Run();

        }
    }
}
