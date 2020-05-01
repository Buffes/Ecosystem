using Unity.Entities;
using Ecosystem.ECS.Animal;
using Ecosystem.Genetics;
using Unity.Transforms;
using UnityEngine;
using Ecosystem.ParticleSystems;

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
                PregnancyData pregnancyData,
                DNA dna,
                AnimalPrefab prefab,
                in Translation position,
                in Rotation rotation) =>
            {
                ParticleMono.InstantiateParticles(ParticleMono.birth, position.Value, 2f);
                Attributes.Animal baby = Object.Instantiate(prefab.Prefab, position.Value, rotation.Value).GetComponent<Attributes.Animal>(); // Spawns child
                baby.InitDNA(pregnancyData.DNAforBaby); // Initialize the baby's DNA 
                EntityManager.RemoveComponent<BirthEvent>(entity);
                EntityManager.RemoveComponent<PregnancyData>(entity);

            }).Run();
        }
    }
}
