using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Targeting.Results;
using Ecosystem.ECS.Targeting.Sensors;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

namespace Ecosystem.ECS.Targeting
{
    /// <summary>
    /// Looks for nearby predators and stores info about the closest predator that was found.
    /// </summary>
    public class FindPredatorSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        private EntityQuery query;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

            query = GetEntityQuery(
                ComponentType.ReadOnly<Translation>(),
                ComponentType.ReadOnly<PreyTypesElement>());
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            var entities = query.ToEntityArray(Allocator.TempJob);
            var positions = query.ToComponentDataArray<Translation>(Allocator.TempJob);
            var preyTypeBuffers = GetBufferFromEntity<PreyTypesElement>();

            Entities
                .WithAll<LookingForPredator>()
                .WithReadOnly(entities)
                .WithReadOnly(positions)
                .WithReadOnly(preyTypeBuffers)
                .ForEach((Entity entity, int entityInQueryIndex,
                in Translation position,
                in Hearing hearing,
                in AnimalTypeData animalType) =>
            {

                int closestPredatorIndex = -1;
                float closestPredatorDistance = 0f;

                // Check all food that we can sense
                for (int i = 0; i < entities.Length; i++)
                {
                    DynamicBuffer<PreyTypesElement> targetPreyTypes = preyTypeBuffers[entities[i]];
                    float3 targetPosition = positions[i].Value;
                    float targetDistance = math.distance(targetPosition, position.Value);

                    if (targetDistance > hearing.Range) continue; // Out of range
                    if (!IsPrey(animalType, targetPreyTypes)) continue; // Not prey to the target
                    if (closestPredatorIndex != -1 && targetDistance >= closestPredatorDistance) continue; // Not the closest

                    closestPredatorIndex = i;
                    closestPredatorDistance = targetDistance;
                }

                // Set result
                if (closestPredatorIndex != -1)
                {
                    commandBuffer.AddComponent(entityInQueryIndex, entity, new FoundPredator
                    {
                        Entity = entities[closestPredatorIndex],
                        Position = positions[closestPredatorIndex].Value
                    });
                }
                else
                {
                    commandBuffer.RemoveComponent<FoundPredator>(entityInQueryIndex, entity);
                }

            }).ScheduleParallel();

            entities.Dispose(Dependency);
            positions.Dispose(Dependency);

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }

        private static bool IsPrey(AnimalTypeData animalType, DynamicBuffer<PreyTypesElement> preyBuffer)
        {
            for (int i = 0; i < preyBuffer.Length; i++)
            {
                int prey = preyBuffer[i].AnimalTypeId;

                if (animalType.AnimalTypeId == prey)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
