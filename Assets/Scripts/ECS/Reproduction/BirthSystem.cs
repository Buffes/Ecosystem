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
                .WithoutBurst()
                .WithAll<BirthEvent>()
                .ForEach((Entity entity
                , PregnancyData pregnancyData
                , in Translation position
                , in Rotation rotation
                , in DNA dna
                , in AnimalPrefab prefab) =>
            {

                Attributes.Animal baby = Object.Instantiate(prefab.Prefab, position.Value, rotation.Value); // Spawns child
                baby.InitDNA(DNA.InheritedDNA(dna, pregnancyData.DNAfromFather)); // Initialize the baby's DNA 

            }).Run();
        }
    }
}
