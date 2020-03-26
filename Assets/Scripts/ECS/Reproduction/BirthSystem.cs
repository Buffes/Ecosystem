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
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        private EntityQuery query;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

            query = GetEntityQuery(ComponentType.ReadOnly<EntityArchetype>());
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            Entities.WithAll<BirthEvent>().ForEach((Entity entity, int entityInQueryIndex
                , ref PregnancyData pregnancyData
                , in Translation position
                , in Rotation rotation
                , in DNA dna
                , in AnimalPrefab prefab) =>
            {
                //TODO Spawn new animal of the same type as parents
                DNA newDNA = DNA.InheritedDNA(dna, pregnancyData.DNAfromFather); // inherit genes from parents
                Attributes.Animal baby = Object.Instantiate(prefab.Prefab, position.Value, rotation.Value); // Spawns child
                baby.InitDNA(newDNA);
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
