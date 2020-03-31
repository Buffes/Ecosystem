using Unity.Entities;
using Ecosystem.ECS.Animal;
using Ecosystem.Genetics;
using Ecosystem.ECS.Animal.Needs;

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
                , ref SexualUrgesData sexualUrgesData
                , in SexData sexData
                , in DNA dna) =>
            {
                if(sexData.Sex == Sex.Female && !EntityManager.HasComponent<PregnancyData>(entity))
                {
                    DNA newDNA = DNA.InheritedDNA(dna, reproductionEvent.PartnerDNA);
                    EntityManager.AddComponentData(entity, new PregnancyData { DNAforBaby = newDNA, TimeSinceFertilisation = 0.0f }); // If female, become pregnant
                }
                sexualUrgesData.Urge += 1.0f; // Sate the sexual urge of the animal
                EntityManager.RemoveComponent<ReproductionEvent>(entity);

            }).Run();

        }
    }
}
