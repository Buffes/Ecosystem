using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Targeting.Results;
using Ecosystem.ECS.Targeting.Sensors;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Targeting
{
    /// <summary>
    /// Looks for nearby prey and stores info about the closest prey that was found.
    /// </summary>
    [DisableAutoCreation]
    public class FindPreySystem : SystemBase
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
                .WithAll<LookingForPrey>()
                .WithReadOnly(entities)
                .WithReadOnly(positions)
                .WithReadOnly(animalTypes)
                .ForEach((Entity entity, int entityInQueryIndex,
                in Translation position,
                in Hearing hearing,
                in DynamicBuffer<PreyTypesElement> preyTypeBuffer) =>
            {

                int closestPreyIndex = -1;
                float closestPreyDistance = 0f;

                // Check all animals that we can sense
                for (int i = 0; i < entities.Length; i++)
                {
                    AnimalTypeData targetAnimalType = animalTypes[i];
                    float3 targetPosition = positions[i].Value;
                    float targetDistance = math.distance(targetPosition, position.Value);

                    if (targetDistance > hearing.Range) continue; // Out of range
                    if (!IsPrey(targetAnimalType, preyTypeBuffer)) continue; // Not prey
                    if (closestPreyIndex != -1 && targetDistance >= closestPreyDistance) continue; // Not the closest

                    closestPreyIndex = i;
                    closestPreyDistance = targetDistance;
                }

                // Set result
                if (closestPreyIndex != -1)
                {
                    commandBuffer.AddComponent(entityInQueryIndex, entity, new FoundPrey
                    {
                        Position = positions[closestPreyIndex].Value
                    });
                }
                else
                {
                    commandBuffer.RemoveComponent<FoundPrey>(entityInQueryIndex, entity);
                }

            }).ScheduleParallel();

            entities.Dispose(Dependency);
            positions.Dispose(Dependency);
            animalTypes.Dispose(Dependency);

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
