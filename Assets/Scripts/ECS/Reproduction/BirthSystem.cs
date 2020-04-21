using Unity.Entities;
using Ecosystem.ECS.Animal;
using Ecosystem.Genetics;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.ECS.Reproduction
{
    /// <summary>
    /// System for handling births. Affects entities with the BirthEvent component.
    /// </summary>
    public class BirthSystem : SystemBase
    {

        protected override void OnUpdate()
        {
            Entities
                .WithStructuralChanges()
                .WithoutBurst()
                .WithAll<BirthEvent>()
                .ForEach((Entity entity,
                PregnancyParticleSystem particleSystem,
                PregnancyData pregnancyData,
                DNA dna,
                AnimalPrefab prefab,
                in Translation position,
                in Rotation rotation) =>
            {

                Attributes.Animal baby = Object.Instantiate(prefab.Prefab, position.Value, rotation.Value); // Spawns child
                baby.InitDNA(pregnancyData.DNAforBaby); // Initialize the baby's DNA 
                EntityManager.RemoveComponent<BirthEvent>(entity);
                EntityManager.RemoveComponent<PregnancyData>(entity);
                if (particleSystem.ParticleSystem.IsAlive())
                    particleSystem.ParticleSystem.Stop();

            }).Run();
        }
    }
}
