using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Targeting.Sensors;
using Ecosystem.ECS.Targeting.Targets;

using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Targeting.FindSystems
{
    /// <summary>
    /// Looks for nearby mates and stores info about the closest mate that was found.
    /// </summary>
    public class FindMateSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        private EntityQuery query;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

            query = GetEntityQuery(
                ComponentType.ReadOnly<Translation>(),
                ComponentType.ReadOnly<AnimalTypeData>());
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            var entities = query.ToEntityArray(Allocator.TempJob);
            var positions = query.ToComponentDataArray<Translation>(Allocator.TempJob);
            var animalTypes = query.ToComponentDataArray<AnimalTypeData>(Allocator.TempJob);

            Entities
                .WithReadOnly(entities)
                .WithReadOnly(positions)
                .WithReadOnly(animalTypes)
                .ForEach((Entity entity, int entityInQueryIndex,
                ref LookingForMate lookingForMate,
                in Translation position,
                in Hearing hearing) =>
                {

                }).ScheduleParallel();
        }
    }
}
